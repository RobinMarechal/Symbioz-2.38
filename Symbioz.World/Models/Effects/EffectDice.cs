using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectDice : Effect {
        public ushort Min { get; set; }

        public ushort Max { get; set; }

        public ushort Const { get; set; }

        public EffectDice() { }

        public EffectDice(ushort effectId, ushort min, ushort max, ushort @const)
            : base(effectId) {
            this.Min = min;
            this.Max = max;
            this.Const = @const;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectDice(this.EffectId, this.Min, this.Max, this.Const);
        }

        public override bool Equals(object obj) {
            return obj is EffectDice dice && this.Equals(dice);
        }

        public bool Equals(EffectDice effect) {
            return this.EffectId == effect.EffectId && effect.Min == this.Min && effect.Max == this.Max && effect.Const == this.Const;
        }
    }
}