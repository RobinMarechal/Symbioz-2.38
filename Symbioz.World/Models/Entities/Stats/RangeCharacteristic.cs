using YAXLib;

#pragma warning disable 659

namespace Symbioz.World.Models.Entities.Stats {
    public class RangeCharacteristic : LimitCharacteristic {
        public const short RANGE_LIMIT = 6;

        [YAXDontSerialize]
        public override short Limit => RANGE_LIMIT;

        [YAXDontSerialize]
        public override bool ContextLimit => true;

        public new static RangeCharacteristic New(short @base) {
            return new RangeCharacteristic {
                Base = @base,
                Additional = 0,
                Context = 0,
                Objects = 0
            };
        }

        public new static RangeCharacteristic Zero() {
            return New(0);
        }

        public override Characteristic Clone() {
            return new RangeCharacteristic {
                Additional = this.Additional,
                Base = this.Base,
                Context = this.Context,
                Objects = this.Objects
            };
        }
    }
}