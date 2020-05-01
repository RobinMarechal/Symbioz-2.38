using System.Collections.Generic;
using System.Linq;
using Symbioz.Core;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Fights.Results;
using Symbioz.World.Models.Items;
using Symbioz.World.Models.Monsters;
using Symbioz.World.Providers;
using Symbioz.World.Records.Monsters;

namespace Symbioz.World.Models.Fights.Fighters {
    public class MonsterFighter : BrainFighter {
        private readonly Dictionary<MonsterDrop, int> m_dropsCount = new Dictionary<MonsterDrop, int>();

        public MonsterFighter(FightTeam team, Monster monster, ushort mapCellId)
            : base(team, mapCellId, monster.Template, monster.GradeId) { }

        public MonsterFighter(FightTeam team, MonsterRecord template, sbyte gradeId, ushort mapCellId)
            : base(team, mapCellId, template, gradeId) { }

        public override GameFightFighterInformations GetFightFighterInformations() {
            return new GameFightMonsterInformations(this.Id,
                                                    this.Look.ToEntityLook(),
                                                    new EntityDispositionInformations((short) this.CellId, (sbyte) this.Direction),
                                                    this.Team.Id,
                                                    0,
                                                    this.Alive,
                                                    this.Stats.GetFightMinimalStats(),
                                                    new ushort[0],
                                                    this.Template.Id,
                                                    this.GradeId);
        }

        public override uint GetDroppedKamas() {
            AsyncRandom asyncRandom = new AsyncRandom();
            return (uint) asyncRandom.Next(this.Template.MinDroppedKamas, this.Template.MaxDroppedKamas + 1);
        }

        public override void OnTurnStarted() {
            base.OnTurnStarted();

            this.PassTurn();
        }

        public override IEnumerable<DroppedItem> RollLoot(int teamPp, int dropBonusPercent) {
            if (this.Alive) {
                return new DroppedItem[0];
            }

            AsyncRandom asyncRandom = new AsyncRandom();
            List<DroppedItem> list = new List<DroppedItem>();
            int prospectingSum = this.OposedTeam().GetFighters<CharacterFighter>().Sum(entry => entry.Stats.Prospecting.TotalInContext());
            
            IEnumerable<MonsterDrop> monsterDroppableItems = from droppableItem in this.Template.Drops
                                                    where prospectingSum >= droppableItem.ProspectingLock && !droppableItem.HasCriteria
                                                    select droppableItem;
            foreach (MonsterDrop droppableItem in monsterDroppableItems) {
                int attempts = 0;
                while (attempts < droppableItem.Count && (droppableItem.DropLimit <= 0 || !this.m_dropsCount.ContainsKey(droppableItem) || this.m_dropsCount[droppableItem] < droppableItem.DropLimit)) {
                    double randomDropThreshold = asyncRandom.Next(0, 100) + asyncRandom.NextDouble();
                    double randomDropChance = FormulasProvider.Instance.AdjustDropChance(teamPp, droppableItem, this.GradeId, this.Fight.AgeBonus, dropBonusPercent);

                    // Probability to drop
                    if (randomDropChance >= randomDropThreshold) {
                        // Item dropped, add it to list
                        list.Add(new DroppedItem(droppableItem.ItemId, 1u));
                        
                        // Update the map for while conditions
                        if (!this.m_dropsCount.ContainsKey(droppableItem)) {
                            this.m_dropsCount.Add(droppableItem, 1);
                        }
                        else {
                            Dictionary<MonsterDrop, int> dropsCount;
                            MonsterDrop key;
                            (dropsCount = this.m_dropsCount)[key = droppableItem] = dropsCount[key] + 1;
                        }
                    }

                    attempts++;
                }
            }

            return list;
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformation() {
            return new FightTeamMemberMonsterInformations((double) this.Id, (int) this.Template.Id, this.GradeId);
        }
    }
}