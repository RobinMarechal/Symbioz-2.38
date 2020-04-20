using Symbioz.Protocol.Selfmade.Enums;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Models.Maps;
using Symbioz.World.Providers.Fights.Buffs;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Spells.Eniripsa {
    [CustomSpellHandler(129)]
    public class Lapino : CustomSpellHandler {
        private Fighter Summon;

        public Lapino(Fighter source, SpellLevelRecord level, MapPoint castPoint, bool criticalHit)
            : base(source, level, castPoint, criticalHit) { }

        public override void Execute() {
            var effects = this.GetEffects().ToList();
            var effect = effects[1];
            effects.RemoveAt(1);
            this.DefaultHandler(effects);
            this.Summon = this.Source.GetLastSummon();
            this.AddTriggerBuff(this.Summon, 0, TriggerType.BEFORE_DEATH, this.Level, effect, this.Level.SpellId, 0, this.OnLapinoDie, -1);
        }

        private bool OnLapinoDie(TriggerBuff buff, TriggerType trigger, object token) {
            this.DefaultHandler(new EffectInstance[] { this.GetEffects().ToArray()[1]}, this.Summon.Point);

            return false;
        }
    }
}