using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectString : Effect {
        public string Value { get; set; }

        public EffectString() { }

        public EffectString(ushort effectId, string value)
            : base(effectId) {
            this.Value = value;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectString(this.EffectId, this.Value);
        }

        public override bool Equals(object obj) {
            return obj is EffectString effect && this.Equals(effect);
        }

        public bool Equals(EffectString effect) {
            return this.EffectId == effect.EffectId && this.Value == effect.Value;
        }
    }
}