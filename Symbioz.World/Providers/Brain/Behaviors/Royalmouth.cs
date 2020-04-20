using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Messages;
using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Fights.FightModels;
using Symbioz.World.Models.Maps;
using Symbioz.World.Providers.Fights.Buffs;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Providers.Brain.Actions;

namespace Symbioz.World.Providers.Brain.Behaviors {
    [Behavior("Royalmouth")]
    public class Royalmouth : Behavior {
        public const ushort InitmouthSpellId = 2356;

        private SpellRecord InitmouthRecord { get; set; }
        private SpellLevelRecord InitmouthLevel { get; set; }

        private bool IsInitmouthed {
            get { return this.Fighter.HasState(x => x.Invulnerable); }
        }

        public short MpBuffAmount {
            get { return (short) this.InitmouthLevel.Effects.FirstOrDefault(x => x.EffectEnum == EffectsEnum.Effect_AddMP_128).DiceMin; }
        }

        public EffectInstance InvulnerabilityEffect {
            get { return this.InitmouthLevel.Effects.FirstOrDefault(x => x.EffectEnum == EffectsEnum.Effect_AddState); }
        }

        public Royalmouth(BrainFighter fighter)
            : base(fighter) {
            this.Fighter.Fight.FightStartEvt += this.Fight_FightStart;
            this.Fighter.AfterSlideEvt += this.Fighter_OnSlideEvt;
            this.Fighter.OnPushDamages += this.Fighter_OnPushDamages;
            this.Fighter.OnTurnStartEvt += this.fighter_OnTurnStartEvt;
            this.InitmouthRecord = SpellRecord.GetSpellRecord(InitmouthSpellId);
            this.InitmouthLevel = this.InitmouthRecord.GetLastLevel();
        }

        void Fighter_OnPushDamages(Fighter obj, Fighter source, short delta, sbyte cellsCount, bool headOn) {
            if (source.Point.IsInLine(this.Fighter.Point)) {
                this.KillsFightersInLine(this.Fighter.CellId, this.Fighter.Point.OrientationTo(source.Point, false));
            }
        }

        void Fighter_OnSlideEvt(Fighter obj, Fighter source, short startCellId, short endCellId) {
            if (this.IsInitmouthed) {
                this.DebuffInitmouth();
                this.RegainMpOnSlided();
            }
        }

        void fighter_OnTurnStartEvt(Fighter obj) {
            bool seq = this.Fighter.Fight.SequencesManager.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
            if (!this.IsInitmouthed)
                this.AddInvulnerabilityBuff();

            if (seq) this.Fighter.Fight.SequencesManager.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }


        void Fight_FightStart(Fight fight) {
            this.Inimouth();
        }

        /// <summary>
        /// Tue les joueurs en ligne lorsque le Royalmouth est poussé contre un obstacle
        /// </summary>
        /// <param name="startCellId"></param>
        /// <param name="endCellId"></param>
        private void KillsFightersInLine(short startCellId, DirectionsEnum direction) {
            MapPoint startPoint = new MapPoint(startCellId);
            MapPoint point2 = new MapPoint(startCellId);
            short i = 1;

            while (point2 != null) {
                Fighter target = this.Fighter.Fight.GetFighter(point2);

                if (target != null && this.Fighter.OposedTeam() == target.Team) {
                    target.Stats.CurrentLifePoints = 0;
                }

                point2 = startPoint.GetCellInDirection(direction, i);
                i++;
            }

            this.Fighter.Fight.CheckDeads();
        }

        /// <summary>
        /// Supprime les effets de l'invulnerabilité donné par Initmouth.
        /// </summary>
        public void DebuffInitmouth() {
            this.Fighter.DispellSpellBuffs(this.Fighter, this.InitmouthRecord.Id);
        }

        /// <summary>
        /// Initialize le Royalmouth au début du combat
        /// </summary>
        private void Inimouth() {
            this.Fighter.CastSpell(this.InitmouthRecord, this.InitmouthRecord.GetLastLevelGrade(), this.Fighter.CellId, this.Fighter.CellId, false);
        }

        /// <summary>
        /// Ajoute au royalmouth un buff d'invulnérabilité 
        /// </summary>
        private void AddInvulnerabilityBuff() {
            StateBuff buff = new StateBuff(this.Fighter.BuffIdProvider.Pop(),
                                           this.Fighter,
                                           this.Fighter,
                                           this.InitmouthLevel,
                                           this.InvulnerabilityEffect,
                                           InitmouthSpellId,
                                           false,
                                           FightDispellableEnum.REALLY_NOT_DISPELLABLE,
                                           SpellStateRecord.GetState(this.InvulnerabilityEffect.Value));
            this.Fighter.AddAndApplyBuff(buff);
        }

        /// <summary>
        /// Ajoute au royalmouth les points de mouvement apres avoir été poussé
        /// </summary>
        private void RegainMpOnSlided() {
            this.Fighter.RegainMp(this.Fighter.Id, this.MpBuffAmount);
        }

        public override ActionType[] GetSortedActions() {
            return new ActionType[] {ActionType.MoveToEnemy, ActionType.CastSpell};
        }
    }
}