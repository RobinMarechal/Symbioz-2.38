using Symbioz.World.Models.Effects;
using Symbioz.World.Records;
using System;
using System.Collections;
using System.Collections.Generic;
using Symbioz.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Protocol.Types;
using Symbioz.Protocol.Enums;

namespace Symbioz.World.Models.Items
{
    public class ItemCollection<T> where T : AbstractItem
    {
        private List<T> m_items = new List<T>();

        public ItemCollection(IEnumerable<T> items)
        {
            this.m_items = items.ToList();

        }
        public ItemCollection()
        {

        }
        public T[] GetItems()
        {
            return this.m_items.ToArray();
        }
        public T[] GetItems(Predicate<T> predicate)
        {
            return this.m_items.FindAll(predicate).ToArray();
        }

        public event Action<T> OnItemAdded;

        public event Action<T, uint> OnItemStacked;

        public event Action<T> OnItemRemoved;

        public event Action<T, uint> OnItemUnstacked;

        public event Action<IEnumerable<T>> OnItemsAdded;

        public event Action<IEnumerable<T>> OnItemsStackeds;

        public event Action<IEnumerable<T>> OnItemsRemoved;

        public event Action<IEnumerable<T>> OnItemsUnstackeds;

        public event Action<T, uint> OnItemQuantityChanged;

        public event Action<IEnumerable<T>> OnItemsQuantityChanged;

        public event Action OnItemsCollectionChanged;

        public virtual void AddItems(IEnumerable<T> items)
        {
            List<T> addedItems = new List<T>();
            List<T> stackedItems = new List<T>();

            foreach (var item in items)
            {
                T sameItem = this.GetSameItem(item.GId, item.Effects);

                if (sameItem != null)
                {
                    sameItem.Quantity += item.Quantity;
                    stackedItems.Add(item);

                }
                else
                {
                    this.m_items.Add(item);
                    addedItems.Add(item);
                }
            }

            if (this.OnItemAdded != null) this.OnItemsAdded(addedItems);

            if (this.OnItemStacked != null) this.OnItemsStackeds(stackedItems);

            if (this.OnItemsQuantityChanged != null) this.OnItemsQuantityChanged(stackedItems);

            if (this.OnItemsCollectionChanged != null) this.OnItemsCollectionChanged();
        }
        
        public virtual void RemoveItems(Dictionary<uint, uint> informations)
        {
            List<T> removedItems = new List<T>();
            List<T> unstackedItems = new List<T>();

            foreach (var info in informations)
            {
                T item = this.GetItem(info.Key);

                if (item != null)
                {
                    if (item.Quantity == info.Value)
                    {
                        this.m_items.Remove(item);
                        removedItems.Add(item);
                    }
                    else
                    {
                        item.Quantity -= info.Value;
                        unstackedItems.Add(item);
                    }
                }
            }

            if (this.OnItemsRemoved != null) this.OnItemsRemoved(removedItems);

            if (this.OnItemsUnstackeds != null) this.OnItemsUnstackeds(unstackedItems);

            if (this.OnItemsQuantityChanged != null) this.OnItemsQuantityChanged(unstackedItems);

            if (this.OnItemsCollectionChanged != null) this.OnItemsCollectionChanged();

        }
        public virtual void AddItem(T item)
        {
            T sameItem = this.GetSameItem(item.GId, item.Effects);
            
            if (sameItem != null)
            {
                sameItem.Quantity += item.Quantity;
                if (this.OnItemStacked != null) this.OnItemStacked(sameItem, item.Quantity);

                if (this.OnItemQuantityChanged != null) this.OnItemQuantityChanged(sameItem, item.Quantity);
            }
            else
            {
                this.m_items.Add(item);
                if (this.OnItemAdded != null) this.OnItemAdded(item);
            }

            if (this.OnItemsCollectionChanged != null) this.OnItemsCollectionChanged();
        }

        public virtual void AddItem(T item, uint quantity)
        {
            T sameItem = this.GetSameItem(item.GId, item.Effects);

            if (sameItem != null)
            {
                sameItem.Quantity += quantity;
                if (this.OnItemStacked != null) this.OnItemStacked(sameItem, quantity);

                if (this.OnItemQuantityChanged != null) this.OnItemQuantityChanged(sameItem, quantity);
            }
            else
            {
                item = (T)item.CloneWithUID();
                item.Quantity = quantity;
                this.m_items.Add(item);

                if (this.OnItemAdded != null) this.OnItemAdded(item);
            }

            if (this.OnItemsCollectionChanged != null) this.OnItemsCollectionChanged();
        }
        public virtual void RemoveItem(T item, uint quantity)
        {
            if (item != null)
            {
                if (item.PositionEnum != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                    return;

                if (item.Quantity >= quantity)
                {
                    if (item.Quantity == quantity)
                    {
                        this.m_items.Remove(item);
                        if (this.OnItemRemoved != null) this.OnItemRemoved(item);
                    }
                    else
                    {
                        item.Quantity -= quantity;
                        if (this.OnItemUnstacked != null) this.OnItemUnstacked(item, quantity);

                        if (this.OnItemQuantityChanged != null) this.OnItemQuantityChanged(item, quantity);
                    }
                }
            }

            if (this.OnItemsCollectionChanged != null) this.OnItemsCollectionChanged();

        }
        public void RemoveItem(uint uid)
        {
            T item = this.GetItem(uid);
            this.RemoveItem(item, item.Quantity);
        }
        public void RemoveItem(uint uid, uint quantity)
        {
            T item = this.GetItem(uid);
            this.RemoveItem(item, quantity);
        }
        public bool Contains(T item)
        {
            return this.m_items.Contains(item);
        }
        protected virtual T GetSameItem(ushort gid, List<Effect> effects)
        {
            return this.GetItems().FirstOrDefault(x => x.GId == gid && SameEffects(effects, x.Effects));
        }
        public void Clear(bool instant)
        {
            Dictionary<uint, uint> informations = this.m_items.ToDictionary(x => x.UId, y => y.Quantity);

            if (instant)
            {
                this.RemoveItems(informations);
            }
            else
            {
                foreach (var info in informations)
                {
                    this.RemoveItem(info.Key, info.Value);
                }
            }
        }
        public T GetItem(uint uid)
        {
            return this.m_items.Find(x => x.UId == uid);
        }
        public T GetItem(ushort gid, List<Effect> effects)
        {
            return this.GetSameItem(gid, effects);
        }
        public static bool SameEffects(List<Effect> e1, List<Effect> e2)
        {
            return e1.SequenceEqual(e2);
        }
        public ObjectItem[] GetObjectsItems()
        {
            var array = Array.ConvertAll<T, ObjectItem>(this.GetItems(), x => x.GetObjectItem());
            return array;
        }
        public bool Exist(ushort gid, uint minimumQuantity)
        {
            return this.m_items.Find(x => x.GId == gid && x.Quantity >= minimumQuantity) != null;
        }
        public bool Exist(ushort gId)
        {
            return this.m_items.Find(x => x.GId == gId) != null;
        }    
        public static Dictionary<List<T>, List<Effect>> SortByEffects(List<T> items)
        {
            Dictionary<List<T>, List<Effect>> results = new Dictionary<List<T>, List<Effect>>();
            foreach (var item in items)
            {
                var same = results.FirstOrDefault(x => SameEffects(x.Value, item.Effects));

                if (same.Key == null)
                    results.Add(new List<T>() { item }, item.Effects);
                else
                    same.Key.Add(item);
            }
            return results;
        }

    }
}