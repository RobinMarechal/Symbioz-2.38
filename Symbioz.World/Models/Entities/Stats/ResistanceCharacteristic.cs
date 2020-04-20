using YAXLib;

#pragma warning disable 659

namespace Symbioz.World.Models.Entities.Stats {
    public class ResistanceCharacteristic : LimitCharacteristic {
        public const short RESISTANCE_PERCENTAGE_LIMIT = 50;

        [YAXDontSerialize]
        public override short Limit => RESISTANCE_PERCENTAGE_LIMIT;

        [YAXDontSerialize]
        public override bool ContextLimit => true;

        public static new ResistanceCharacteristic New(short @base) {
            return new ResistanceCharacteristic() {
                Base = @base,
                Additional = 0,
                Context = 0,
                Objects = 0
            };
        }

        public new static ResistanceCharacteristic Zero() {
            return New(0);
        }

        public override Characteristic Clone() {
            return new ResistanceCharacteristic {
                Additional = this.Additional,
                Base = this.Base,
                Context = this.Context,
                Objects = this.Objects
            };
        }
    }
}