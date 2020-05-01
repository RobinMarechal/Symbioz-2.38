using YAXLib;

namespace Symbioz.World.Models.Entities.Stats {
    public abstract class LimitCharacteristic : Characteristic {
        [YAXDontSerialize]
        public abstract short Limit { get; }

        [YAXDontSerialize]
        public abstract bool ContextLimit { get; }

        public override short Total() {
            if (this.ContextLimit) {
                short total = base.Total();
                return total > this.Limit ? this.Limit : total;
            }

            return base.Total();
        }
    }
}