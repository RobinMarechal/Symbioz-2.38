using Symbioz.Core;
using Symbioz.Protocol.Types;
using Symbioz.World.Records;
using YAXLib;

#pragma warning disable 659

namespace Symbioz.World.Models.Effects {
    public class EffectMinationLevel : Effect {
        public const ushort DUMMY_TEXT_EFFECT_ID = 990;

        public const string EFFECT_MESSAGE = "Niveau {0} ({1}%)";

        public ushort Level { get; set; }
        public ushort Percentage { get; set; }
        public ulong Exp { get; set; }

        [YAXDontSerialize]
        public ulong LowerBoundExperience {
            get { return ExperienceRecord.GetExperienceForLevel(this.Level).Player; }
        }

        [YAXDontSerialize]
        public ulong UpperBoundExperience {
            get { return ExperienceRecord.GetExperienceForNextLevel(this.Level).Player; }
        }

        public EffectMinationLevel() { }

        public EffectMinationLevel(ushort level, ushort percentage, ulong exp)
            : base(DUMMY_TEXT_EFFECT_ID) {
            this.Level = level;
            this.Percentage = percentage;
            this.Exp = exp;
        }

        public override ObjectEffect GetObjectEffect() {
            return new ObjectEffectString(this.EffectId, string.Format(EFFECT_MESSAGE, this.Level, this.Percentage));
        }

        public override bool Equals(object obj) {
            return obj is EffectMinationLevel ? this.Equals(obj as EffectMinationLevel) : false;
        }

        public bool Equals(EffectMinationLevel effect) {
            return this.EffectId == effect.EffectId && this.Level == effect.Level && this.Percentage == effect.Percentage;
        }

        public void AddExperience(ulong value) {
            if (this.Level >= ExperienceRecord.MaxMinationLevel) {
                return;
            }

            this.Exp += value;


            if (this.Exp >= this.UpperBoundExperience || this.Exp < this.LowerBoundExperience) {
                this.Level = ExperienceRecord.GetCharacterLevel(this.Exp);
            }

            long neededToUp = (long) (this.UpperBoundExperience - this.LowerBoundExperience);
            long current = (neededToUp) - ((long) this.UpperBoundExperience - (long) this.Exp);
            this.Percentage = (ushort) Extensions.Percentage(current, neededToUp);

            if (this.Level >= ExperienceRecord.MaxMinationLevel) {
                this.Level = ExperienceRecord.MaxMinationLevel;
                this.Exp = ExperienceRecord.GetExperienceForLevel(ExperienceRecord.MaxMinationLevel).Player;
                this.Percentage = 0;
            }
        }
    }
}