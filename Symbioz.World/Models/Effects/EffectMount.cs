using System;
using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectMount : Effect {
        public int MountId { get; set; }

        public double Date { get; set; }

        public ushort ModelId { get; set; }

        public EffectMount() { }

        public EffectMount(ushort effectId, int mountId, double date, ushort modelId) : base(effectId) {
            this.MountId = mountId;
            this.Date = date;
            this.ModelId = modelId;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectMount(this.EffectId, this.MountId, this.Date, this.ModelId);
        }

        public override bool Equals(object obj) {
            return obj is EffectMount ? this.Equals(obj as EffectMount) : false;
        }

        public bool Equals(EffectMount effect) {
            return this.EffectId == effect.EffectId && this.MountId == effect.MountId && this.ModelId == effect.ModelId && Math.Abs(this.Date - effect.Date) < 0.0001;
        }
    }
}