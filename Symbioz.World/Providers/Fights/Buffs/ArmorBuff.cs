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
    public class ArmorBuff : Buff {
        public short Value { get; set; }
        public short Delta { get; set; }

        public override void Apply() {
            this.Target.Stats.GlobalDamageReduction += this.Delta;
        }

        public override void Dispell() {
            this.Target.Stats.GlobalDamageReduction -= this.Delta;
        }

        public ArmorBuff(int id, Fighter target, Fighter caster, SpellLevelRecord level, EffectInstance effect, ushort spellId, short value, bool critical, FightDispellableEnum dispelable)
            : base(id, target, caster, level, effect, spellId, critical, dispelable) {
            this.Value = value;
            this.Delta = (short) target.CalculateArmorValue(this.Value);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect() {
            return new FightTemporaryBoostEffect((uint) this.Id, this.Target.Id, this.Duration, (sbyte) this.Dispelable, this.SpellId, 0, 0, Math.Abs(this.Value));
        }
    }
}