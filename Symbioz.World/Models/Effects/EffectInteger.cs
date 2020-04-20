using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectInteger : Effect {
        public ushort Value { get; set; }

        public EffectInteger() { }

        public EffectInteger(ushort effectId, ushort value)
            : base(effectId) {
            this.Value = value;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectInteger(this.EffectId, this.Value);
        }

        public override bool Equals(object obj) {
            return obj is EffectInteger effect && this.Equals(effect);
        }

        public bool Equals(EffectInteger effect) {
            return this.EffectId == effect.EffectId && effect.Value == this.Value;
        }
    }
}