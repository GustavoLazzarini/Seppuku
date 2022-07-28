//Created by Galactspace

using System;
using Scriptable.Item;

namespace Core.Entities
{
    public struct InventoryItem : IEquatable<InventoryItem>
    {
        public ItemSo Item;
        public int Quantity;

        public event Action OnEmpty;

        public InventoryItem(ItemSo item, int quantity)
        {
            Item = item;
            OnEmpty = null;
            Quantity = quantity;
        }

        public void Add(int quantity = 1)
        {
            Quantity += quantity;
        }

        public void Remove(int quantity = 1)
        {
            Quantity -= quantity;
            if (Quantity <= 0)
            {
                Quantity = 0;
                OnEmpty?.Invoke();
            }
        }

        public bool Equals(InventoryItem other)
        {
            if (other.Item != Item) return false;
            if (other.Quantity != Quantity) return false;

            return true;
        }
    }
}