using YAXLib;

#pragma warning disable 659

namespace Symbioz.World.Models.Entities.Stats {
    public class MpCharacteristic : LimitCharacteristic {
        [YAXDontSerialize]
        public override short Limit => WorldConfiguration.Instance.MpLimit;

        [YAXDontSerialize]
        public override bool ContextLimit => false;

        public new static MpCharacteristic New(short @base) {
            return new MpCharacteristic {
                Base = @base,
                Additional = 0,
                Context = 0,
                Objects = 0
            };
        }

        public override Characteristic Clone() {
            return new MpCharacteristic() {
                Additional = this.Additional,
                Base = this.Base,
                Context = this.Context,
                Objects = this.Objects
            };
        }
    }
}