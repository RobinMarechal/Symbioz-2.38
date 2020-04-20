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
    public class SpellBoost : Buff {
        public SpellBoost(int id, Fighter target, Fighter caster, SpellLevelRecord level, EffectInstance effect, ushort spellId, bool critical, FightDispellableEnum dispelable)
            : base(id, target, caster, level, effect, spellId, critical, dispelable) { }

        public ushort BoostedSpellId {
            get { return this.Effect.DiceMin; }
        }

        public short Boost {
            get { return (short) this.Effect.Value; }
        }

        public override void Apply() {
            this.Target.BuffSpell(this.SpellId, this.Boost);
        }

        public override void Dispell() {
            this.Target.UnBuffSpell(this.SpellId, this.Boost);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect() {
            return new FightTemporarySpellBoostEffect((uint) this.Id, this.Target.Id, this.Duration, (sbyte) this.Dispelable, this.SpellId, 0, 0, this.Boost, this.BoostedSpellId);
        }
    }
}