using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Inventory/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        public List<BaseItemSO> items;

        public void AddItem(BaseItemSO item)
        {
            item.AddToInventory(this);
        }

        public int RemoveItem(BaseItemSO item)
        {
            return item.RemoveFromInventory(this);
        }

        public InventorySO DeepCopy()
        {
            return Instantiate(this);
        }

    }
}