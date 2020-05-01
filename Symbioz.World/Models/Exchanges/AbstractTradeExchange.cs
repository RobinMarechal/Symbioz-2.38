using System.Collections.Generic;
using System.Linq;
using Symbioz.World.Models.Entities;
using Symbioz.World.Models.Items;

namespace Symbioz.World.Models.Exchanges {
    public abstract class AbstractTradeExchange : Exchange {
        public AbstractTradeExchange(Character character) : base(character) { }

        public abstract IEnumerable<ItemStack> GetAllPresentItems();

        public void RemoveAllItems() {
            IEnumerable<ItemStack> allPresentItems = this.GetAllPresentItems();
            foreach (ItemStack item in allPresentItems) {
                this.MoveItem(item.ItemUId,  -1 * (int) item.Quantity);
            }
        }
    }
}