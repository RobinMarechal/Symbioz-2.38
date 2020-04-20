using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectCreature : Effect {
        public ushort MonsterFamilyId { get; set; }

        public EffectCreature() { }

        public EffectCreature(ushort effectId, ushort monsterFamilyId)
            : base(effectId) {
            this.MonsterFamilyId = monsterFamilyId;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectCreature(this.EffectId, this.MonsterFamilyId);
        }

        public override bool Equals(object obj) {
            return obj is EffectCreature ? this.Equals(obj as EffectCreature) : false;
        }

        public bool Equals(EffectCreature effect) {
            return this.EffectId == effect.EffectId && this.MonsterFamilyId == effect.MonsterFamilyId;
        }
    }
}