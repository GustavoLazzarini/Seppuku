//Created by Galactspace

using System;
using System.Linq;
using UnityEngine;
using Scriptable.Item;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Inventory
    {
        private InventoryItem[] _itemArray;
        private Dictionary<string, InventoryItem> _data;

        public event Action<InventoryItem[]> OnUpdate;

        public Inventory(params InventoryItem[] data)
        {
            _data = new();
            for (int i = 0; i < data.Length; i++)
                _data.Add(data[i].Item.name, new());
        }

        public InventoryItem[] GetItens() => _itemArray ??= _data.Values.ToArray();

        public void Add(ItemSo item, int quantity = 1)
        {
            if (quantity < 1) return;

            if (_data.ContainsKey(item.ItemName)) _data[item.ItemName].Add(quantity);
            else _data.Add(item.ItemName, new(item, quantity));
            
            Update();
        }

        public void Remove(string name, int quantity = -1)
        {
            if (!_data.ContainsKey(name) || quantity == 0) return;

            if (quantity < 0) _data.Remove(name);
            else _data[name].Remove(quantity);
            
            Update();
        }
        public void Remove(ItemSo item, int quantity = -1) => Remove(item.ItemName, quantity);
    
        private void Update()
        {
            _itemArray = null;
            OnUpdate?.Invoke(GetItens());
        }
    }
}