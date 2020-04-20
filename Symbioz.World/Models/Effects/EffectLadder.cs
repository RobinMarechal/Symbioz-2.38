using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectLadder : Effect {
        public ushort MonsterFamilyId { get; set; }

        public uint MonsterCount { get; set; }

        public EffectLadder() { }

        public EffectLadder(ushort effectId, ushort monsterFamilyId, uint monsterCount)
            : base(effectId) {
            this.MonsterFamilyId = monsterFamilyId;
            this.MonsterCount = monsterCount;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectLadder(this.EffectId, this.MonsterFamilyId, this.MonsterCount);
        }

        public override bool Equals(object obj) {
            return obj is EffectLadder effect && this.Equals(effect);
        }

        public bool Equals(EffectLadder effect) {
            return this.EffectId == effect.EffectId && this.MonsterFamilyId == effect.MonsterFamilyId && this.MonsterCount == effect.MonsterCount;
        }
    }
}