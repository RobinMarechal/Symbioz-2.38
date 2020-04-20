using Symbioz.Protocol.Types;
using Symbioz.World.Models.Monsters;
using Symbioz.World.Records.Monsters;
#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectMination : Effect {
        public const ushort DUMMY_TEXT_EFFECT_ID = 990;

        public const string EFFECT_MESSAGE = "Contrôle du monstre {0} ({1})";

        public ushort MonsterId { get; set; }
        public sbyte GradeId { get; set; }
        public string MonsterName { get; set; }
        public EffectMination() { }

        public EffectMination(ushort monsterId, string monsterName, sbyte gradeId)
            : base(DUMMY_TEXT_EFFECT_ID) {
            this.MonsterId = monsterId;
            this.MonsterName = monsterName;
            this.GradeId = gradeId;
        }

        public MonsterRecord GetTemplate() {
            return MonsterRecord.GetMonster(this.MonsterId);
        }

        public MonsterGrade GetGrade() {
            return this.GetTemplate().GetGrade(this.GradeId);
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectString(this.EffectId, string.Format(EFFECT_MESSAGE, this.MonsterName, this.GradeId));
        }

        public override bool Equals(object obj) {
            return obj is EffectMination ? this.Equals(obj as EffectMination) : false;
        }


        public bool Equals(EffectMination effect) {
            return this.EffectId == effect.EffectId && this.MonsterId == effect.MonsterId && this.GradeId == effect.GradeId && this.MonsterName == effect.MonsterName;
        }
    }
}