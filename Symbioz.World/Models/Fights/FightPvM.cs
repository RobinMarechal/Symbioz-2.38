using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Core;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Fights.Results;
using Symbioz.World.Models.Items;
using Symbioz.World.Providers;
using Symbioz.World.Providers.Fights.Challenges;
using Symbioz.World.Providers.Maps.Monsters;
using Symbioz.World.Records.Maps;

namespace Symbioz.World.Models.Fights {
    public class FightPvM : Fight {
        public override FightTypeEnum FightType {
            get { return FightTypeEnum.FIGHT_TYPE_PvM; }
        }

        public override bool SpawnJoin {
            get { return true; }
        }

        public override bool PvP {
            get { return false; }
        }

        public override bool ShowBlades {
            get { return true; }
        }

        public MonsterGroup Group { get; set; }
        protected Challenge[] Challenges { get; private set; }

        public override bool MinationAllowed {
            get { return true; }
        }

        public FightPvM(MapRecord map, FightTeam blueTeam, FightTeam redTeam, short cellId, MonsterGroup group) : base(map, blueTeam, redTeam, cellId) {
            this.Group = group;
            this.AgeBonus = group.AgeBonus;
            this.OnFightEndedEvt += this.FightEnded;
            this.FightStartEvt += this.FightPvM_FightStartEvt;
        }

        public override void StartPlacement() {
            base.StartPlacement();
        }

        private void FightPvM_FightStartEvt(Fight obj) { }

        void FightEnded(Fight arg1, bool arg2) {
            if (this.Started && this.Winners == this.GetTeam(TeamTypeEnum.TEAM_TYPE_MONSTER) && this.ShowBlades && !this.GroupExistOnMap()) {
                this.Map.Instance.AddEntity(this.Group);
            }
            else if (!this.Started && this.ShowBlades && !this.GroupExistOnMap()) {
                this.Map.Instance.AddEntity(this.Group);
            }
        }

        public override void Dispose() {
            this.OnFightEndedEvt -= this.FightEnded;
            base.Dispose();
        }

        public bool GroupExistOnMap() {
            var templates = Array.ConvertAll(this.Group.GetMonsters(), x => x.Template);
            return MonsterSpawnManager.Instance.GroupExist(this.Map.Instance, templates);
        }

        public override int GetPreparationDelay() {
            return 30;
        }

        public override void SendGameFightJoinMessage(CharacterFighter fighter) {
            fighter.Character.Client.Send(new GameFightJoinMessage(true, !this.Started, false, this.Started, this.GetPlacementTimeLeft(), (sbyte) this.FightType));
        }

        public virtual FightTeam GetTeamChallenged() {
            return this.RedTeam.Type == TeamTypeEnum.TEAM_TYPE_PLAYER ? this.RedTeam : this.BlueTeam;
        }

        public override void OnFightStarted() {
            this.Challenges = ChallengeProvider.Instance.PopChallenges(this.GetTeamChallenged(), this.GetChallengeCount());
            this.OnChallengePopped();
            base.OnFightStarted();
        }

        private int GetChallengeCount() {
            FightTeam team = this.GetTeamChallenged();
            if (team.GetTeamLevel() >= team.OposedTeam().GetTeamLevel())
                return 1;
            return 2;
        }

        public override FightCommonInformations GetFightCommonInformations() {
            return new FightCommonInformations(this.Id,
                                               (sbyte) this.FightType,
                                               this.GetFightTeamInformations(),
                                               new[] { this.BlueTeam.BladesCellId, this.RedTeam.BladesCellId },
                                               this.GetFightOptionsInformations());
        }

        protected virtual void OnChallengePopped() {
            foreach (var challenge in this.Challenges) {
                this.GetTeamChallenged().Send(new ChallengeInfoMessage(challenge.Id, challenge.GetTargetId(), (uint) challenge.XpBonusPercent, (uint) challenge.DropBonusPercent));
            }
        }

        protected virtual int GetChallengesDropPercentBonus() {
            return Array.FindAll(this.Challenges, x => x.IsSucces()).Sum(x => x.DropBonusPercent);
        }

        protected virtual int GetChallengesExpPercentBonus() {
            return Array.FindAll(this.Challenges, x => x.IsSucces()).Sum(x => x.XpBonusPercent);
        }

        // Read team = players
        // blue team = monsters
        public override Challenge GetChallenge(ushort id) {
            return Array.Find(this.Challenges, x => x.Id == id);
        }

        // TODO: Rewrite drops
        protected override IEnumerable<IFightResult> GenerateResults() {
            List<IFightResult> allFighters = new List<IFightResult>();
            allFighters.AddRange(from entry in this.GetAllFightersWithLeavers()
                                 where !(entry is IOwnable)
                                 select entry.GetFightResult());

            FightTeam[] teams = { this.BlueTeam, this.RedTeam };
            foreach (FightTeam team in teams) {
                int xpBonusPercent = 0;
                int dropBonusPercent = 0;

                if (team == this.GetTeamChallenged()) {
                    xpBonusPercent += this.GetChallengesExpPercentBonus();
                    dropBonusPercent += this.GetChallengesDropPercentBonus();
                }

                FightTeam enemyTeam = team == this.RedTeam ? this.BlueTeam : this.RedTeam;
                IEnumerable<Fighter> deadEnemies = enemyTeam.GetDeads();

                // Retrieve the fighters who can loot, ordered by their pp (asc)
                List<IFightResult> looterFighters = allFighters.FindAll(x => x.CanLoot(team));
                IOrderedEnumerable<IFightResult> orderedLooters = looterFighters.OrderBy(x => x.Prospecting);

                List<Fighter> fighters = team.GetFighters(false);
                int teamPP = fighters.Sum(entry => entry.Stats.Prospecting.TotalInContext());
                teamPP += teamPP.GetPercentageOf(dropBonusPercent);
                
                // drops
                List<DroppedItem> teamLoots = new List<DroppedItem>();
                IEnumerable<DroppedItem> Selector(Fighter dropper) => dropper.RollLoot(teamPP, dropBonusPercent);
                foreach (DroppedItem current in deadEnemies.SelectMany(Selector)) {
                    teamLoots.Add(current);
                }

                long baseKamas = deadEnemies.Sum(entry => (long) ((ulong) entry.GetDroppedKamas()));
                using (IEnumerator<IFightResult> orderedLootersEnumerator = orderedLooters.GetEnumerator()) {

                    while (orderedLootersEnumerator.MoveNext()) {
                        IFightResult looter = orderedLootersEnumerator.Current;

                        // kamas
                        looter.Loot.Kamas = FormulasProvider.Instance.AdjustDroppedKamas(looter, teamPP, baseKamas, dropBonusPercent);

                        // Xp
                        if (looter is FightPlayerResult && looter.Outcome == FightOutcomeEnum.RESULT_VICTORY) {
                            (looter as FightPlayerResult).AddEarnedExperience(xpBonusPercent);
                        }
                    }

                    // Distribution of loots :
                    if (teamLoots.Any()) {
                        AsyncRandom rand = new AsyncRandom();
                        // For each looted item...
                        foreach (DroppedItem item in teamLoots) {
                            // We choose a random looter, but pp still has a little impact on the choice.
                            // For each player, we generate a random number between 0 and its max prospecting in context
                            // We sort the list in ascending order and get the last. We therefore get the fighter with the higher randomly generated number.
                            // Prospecting still has an impact since a player with higher pp has more chances to get a higher random number.
                            Fighter randomLooter = fighters.OrderBy(x => x.Stats.Prospecting.TotalInContext() * rand.NextDouble()).Last();

                            // Give the item looted by the team to this player.
                            randomLooter.Loot.AddItem(item);
                        }
                    }
                }
            }

            return allFighters;
        }
    }
}