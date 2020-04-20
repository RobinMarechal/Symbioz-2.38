using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Types;
using Symbioz.World.Models.Effects;
using Symbioz.World.Models.Fights.Fighters;
using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Providers.Fights.Buffs {
    public class ResistancesBuff : Buff {
        private short Value { get; set; }

        public ResistancesBuff(int id,
                               Fighter target,
                               Fighter caster,
                               SpellLevelRecord level,
                               EffectInstance effect,
                               ushort spellId,
                               bool critical,
                               FightDispellableEnum dispelable,
                               short value)
            : base(id, target, caster, level, effect, spellId, critical, dispelable) {
            this.Value = value;
        }

        public override void Apply() {
            this.Target.Stats.AirResistPercent.Context += this.Value;
            this.Target.Stats.FireResistPercent.Context += this.Value;
            this.Target.Stats.NeutralResistPercent.Context += this.Value;
            this.Target.Stats.WaterResistPercent.Context += this.Value;
        }

        public override void Dispell() {
            this.Target.Stats.AirResistPercent.Context -= this.Value;
            this.Target.Stats.FireResistPercent.Context -= this.Value;
            this.Target.Stats.NeutralResistPercent.Context -= this.Value;
            this.Target.Stats.WaterResistPercent.Context -= this.Value;
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect() {
            return new FightTemporaryBoostEffect((uint) this.Id, this.Target.Id, this.Duration, (sbyte) this.Dispelable, this.SpellId, 0, 0, Math.Abs(this.Value));
        }
    }
}