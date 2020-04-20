using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Records.Spells;
using Symbioz.Core;

namespace Symbioz.World.Providers.Fights.Spells.Ecaflip {
    /// <summary>
    /// Todo
    /// </summary>
    [CustomSpellHandler(114)]
    public class Rekop : CustomSpellHandler {
        private Fighter Target { get; set; }
        public int TurnTrigger { get; private set; }

        public Rekop(Fighter source, SpellLevelRecord level, MapPoint castPoint, bool criticalHit) : base(source, level, castPoint, criticalHit) {
            this.TurnTrigger = new AsyncRandom().Next(1, 4);
            this.Target = this.Source.Fight.GetFighter(this.CastPoint);
        }

        public override void Execute() {
            this.Target.OnTurnStartEvt += this.Target_OnTurnStartEvt;
        }

        private void Target_OnTurnStartEvt(Fighter obj) {
            this.TurnTrigger--;

            if (this.TurnTrigger == 0) {
                obj.Fight.SequencesManager.StartSequence(Protocol.Enums.SequenceTypeEnum.SEQUENCE_SPELL);
                this.DefaultHandler(this.GetEffects().Take(4), this.Target.Point);
                obj.Fight.SequencesManager.EndSequence(Protocol.Enums.SequenceTypeEnum.SEQUENCE_SPELL);
                this.Target.OnTurnStartEvt -= this.Target_OnTurnStartEvt;
            }
        }
    }
}