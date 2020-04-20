using System.Collections.Generic;

namespace Symbioz.World.Models.Items {
    public class ItemCollectionStock<T> : ItemCollection<T> where T : AbstractItem {
        public ItemCollectionStock(List<T> items)
            : base(items) { }

        public ItemCollectionStock() { }

        public void AddItem(T item, uint quantity) {
            base.AddItem(item, quantity);
        }

        public void RemoveItem(uint uid, uint quantity) {
            base.RemoveItem(uid, quantity);
        }
    }
}