using System;
using System.Collections.Generic;
using System.Linq;
using SSync.Messages;
using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Entities.Stats;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Fights.Results;
using Symbioz.World.Models.Fights.Spells;
using Symbioz.World.Models.Maps;
using Symbioz.World.Providers.Items;
using Symbioz.World.Records.Idols;
using Symbioz.World.Records.Items;
using Symbioz.World.Records.Spells;

namespace Symbioz.World.Models.Fights.Fighters {
    public class CharacterFighter : PlayableFighter {
        public event Action OnLeavePreFightEvt;

        public event Action<Fighter> OnWeaponUsedEvt;

        public Character Character { get; set; }

        public override string Name => this.Character.Name;

        public override ushort Level => this.Character.Level;

        public override bool Sex => this.Character.Record.Sex;

        private SpellLevelRecord WeaponLevel { get; set; }
        private WeaponRecord WeaponTemplate { get; set; }
        private bool HasWeaponEquiped => this.WeaponTemplate != null;

        public CharacterFighter(Character character, FightTeam team, ushort mapPosition)
            : base(team, mapPosition) {
            this.Character = character;
        }

        public override void Initialize() {
            this.Id = (int) this.Character.Id;
            this.Look = this.Character.Look.Clone();
            this.Stats = new FighterStats(this.Character);

            if (this.Character.Inventory.HasWeaponEquiped) {
                this.WeaponTemplate = WeaponRecord.GetWeapon(this.Character.Inventory.GetWeapon().GId);
                this.WeaponLevel = WeaponManager.Instance.GetWeaponSpellLevel(this.WeaponTemplate);
            }

            base.Initialize();
        }

        public override void OnTurnStarted() {
            this.SendFighterStatsList();
            base.OnTurnStarted();
        }

        public override void OnJoined() {
            this.Fight.SendGameFightJoinMessage(this);
            this.ShowPlacementCells();
            this.Fight.ShowFighters(this);
            this.ShowReadyFighters();
            base.OnJoined();
        }

        public void ShowReadyFighters() {
            foreach (var fighter in this.Fight.GetFighters<CharacterFighter>().FindAll(x => x.IsReady)) {
                this.Character.Client.Send(new GameFightHumanReadyStateMessage((ulong) fighter.Id, true));
            }
        }

        public void ToggleReady(bool isReady) {
            this.IsReady = isReady;
            this.Fight.OnSetReady(this, this.IsReady);
        }

        public void SendFighterStatsList() {
            this.Character.Client.Send(new FighterStatsListMessage(this.Stats.GetCharacterCharacteristics(this.Character)));
        }

        public IdolRecord[] GetIdols() {
            return this.Character.Record.Idols.ConvertAll(IdolRecord.GetIdol).ToArray();
        }

        public override void Move(List<short> movementKeys) {
            base.Move(movementKeys.ToList());
        }

        public void OnDisconnected() {
            this.Leave(true);
        }

        public void Leave(bool teleportToSpawn) {
            if (!this.Fight.Started) {
                this.Team.RemoveFighter(this);

                this.OnLeavePreFightEvt?.Invoke();


                if (!this.Fight.CheckFightEnd()) {
                    this.Fight.CheckFightStart();
                }

                this.Character.RejoinMap(this.Fight.FightType, false, teleportToSpawn && this.Fight.SpawnJoin, this.Stats);
            }
            else {
                if (!this.Left) {
                    if (this.Alive) {
                        this.Stats.CurrentLifePoints = 0;
                        this.Fight.CheckDeads();
                    }

                    if (!this.Fight.Ended) {
                        Synchronizer sync = new Synchronizer(this.Fight,
                                                             new PlayableFighter[] {
                                                                 this
                                                             });
                        sync.Success += delegate { this.OnPlayerReadyToLeave(); };
                        sync.Timeout += delegate { this.OnPlayerReadyToLeave(); };
                        this.PersonalSynchronizer = sync;
                        sync.Start();
                    }
                    //if (Fight.SequencesManager.SequencesCount == 0)
                    //    OnPlayerReadyToLeave();
                    //else
                    //{
                    //    this.WaitAcknolegement = true;
                    //    this.OnSequencesAcknowleged = OnPlayerReadyToLeave;
                    //}

                    this.Left = true;
                }
            }
        }

        public void ToggleSyncReady(bool isReady) {
            if (this.PersonalSynchronizer != null) {
                this.PersonalSynchronizer.ToggleReady(this, isReady);
            }
            else {
                this.Fight.Synchronizer?.ToggleReady(this, isReady);
            }
        }

        public override void Kick() {
            this.Leave(false);
        }

        public void ShowPlacementCells() {
            this.Send(new GameFightPlacementPossiblePositionsMessage(this.Fight.RedTeam.GetPlacements(), this.Fight.BlueTeam.GetPlacements(), this.Team.Id));
        }

        public override GameFightFighterInformations GetFightFighterInformations() {
            return new GameFightCharacterInformations(this.Id,
                                                      this.Look.ToEntityLook(),
                                                      new EntityDispositionInformations(this.CellId, (sbyte) this.Direction),
                                                      this.Team.Id,
                                                      0,
                                                      this.Alive,
                                                      this.Stats.GetFightMinimalStats(),
                                                      new ushort[0],
                                                      this.Character.Name,
                                                      this.Character.GetPlayerStatus(),
                                                      (byte) this.Character.Level,
                                                      this.Character.Record.Alignment.GetActorAlignmentInformations(),
                                                      this.Character.Record.BreedId,
                                                      this.Character.Record.Sex);
        }


        public override void AddCooldownOnSpell(Fighter source, ushort spellId, short value) {
            base.AddCooldownOnSpell(source, spellId, value);
            this.Character.Client.Send(new GameActionFightSpellCooldownVariationMessage((ushort) ActionsEnum.ACTION_CHARACTER_ADD_SPELL_COOLDOWN,
                                                                                        source.Id,
                                                                                        this.Id,
                                                                                        spellId,
                                                                                        value));
        }

        public override bool CastSpell(SpellRecord spell, sbyte grade, short cellId, int targetId = 0, bool verif = true) {
            if (spell.Id != WeaponManager.PunchSpellId) 
                return base.CastSpell(spell, grade, cellId, targetId, verif);
            
            MapPoint castPoint = new MapPoint(cellId);

            if (this.HasWeaponEquiped) {
                string rawZone = WeaponManager.Instance.GetRawZone(this.WeaponTemplate.Template.TypeEnum);

                return this.CloseCombat(this.WeaponLevel, castPoint, rawZone, this.WeaponTemplate.Id);
            }

            SpellLevelRecord level = this.GetSpell(WeaponManager.PunchSpellId).Template.GetLastLevel();
            return this.CloseCombat(level, castPoint, WeaponManager.PunchRawZone);

        }

        private bool CloseCombat(SpellLevelRecord level, MapPoint castPoint, string rawZone, ushort weaponGId = 0) {
            SpellCastResultEnum canCast = this.CanCastSpell(level, this.CellId, castPoint.CellId);

            if (canCast != SpellCastResultEnum.Ok) {
                this.OnSpellCastFailed(canCast, level);
                return false;
            }

            this.OnWeaponUsedEvt?.Invoke(this);

            this.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

            FightSpellCastCriticalEnum fightSpellCastCriticalEnum = this.RollCriticalDice(level);
            bool criticalHit = fightSpellCastCriticalEnum == FightSpellCastCriticalEnum.CRITICAL_HIT;

            EffectInstance[] effects = (criticalHit ? level.CriticalEffects : level.Effects).ToArray();


            this.Fight.Send(new GameActionFightCloseCombatMessage((ushort) ActionsEnum.ACTION_FIGHT_CLOSE_COMBAT,
                                                                  this.Id,
                                                                  false,
                                                                  false,
                                                                  0,
                                                                  castPoint.CellId,
                                                                  (sbyte) fightSpellCastCriticalEnum,
                                                                  weaponGId));

            SpellEffectsManager.Instance.HandleEffects(this,
                                                       effects,
                                                       level,
                                                       castPoint,
                                                       rawZone,
                                                       WeaponManager.WeaponTargetMask,
                                                       criticalHit);


            this.UseAp(level.ApCost);

            this.OnSpellCasted(level, this.CellId, fightSpellCastCriticalEnum);
            this.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);
            this.Fight.CheckDeads();
            this.Fight.CheckFightEnd();
            return true;
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformation() {
            return new FightTeamMemberCharacterInformations(this.Id, this.Character.Name, (byte) this.Character.Level);
        }

        public void OnPlayerReadyToLeave() {
            this.PersonalSynchronizer = null;

            if (this.Fight != null && !this.Fight.CheckFightEnd()) // ??? Fight != null.??
            {
                this.Team.RemoveFighter(this);
                this.Team.AddLeaver(this);

                if (this.IsFighterTurn) {
                    this.Fight.StopTurn();
                }

                //  fighter.ResetFightProperties();
                this.Character.RejoinMap(this.Fight.FightType, false, this.Fight.SpawnJoin, this.Stats);
            }
        }

        public override CharacterSpell GetSpell(ushort spellId) {
            return this.Character.GetSpell(spellId);
        }

        public override IFightResult GetFightResult() {
            return new FightPlayerResult(this, this.GetFighterOutcome(), this.Loot);
        }


        public override void Send(Message message) {
            this.Character.Client.Send(message);
        }

        public override Character GetCharacterPlaying() {
            return this.Character;
        }
    }
}