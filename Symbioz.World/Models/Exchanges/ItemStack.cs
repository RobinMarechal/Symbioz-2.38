using Symbioz.World.Models.Entities;
using Symbioz.World.Records.Characters;

namespace Symbioz.World.Models.Exchanges {
    public struct ItemStack {
        public uint ItemUId;
        public uint Quantity;

        public ItemStack(uint itemUId, uint quantity) {
            this.ItemUId = itemUId;
            this.Quantity = quantity;
        }
    }
}