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

        public BaseItemSO handItem;

        public event Action<BaseItemSO> OnItemAdded;
        public event Action<BaseItemSO, int> OnItemRemoved;
        public AnimatorSO animator;

        private void OnEnable()
        {
            InitiateInventory();
        }

        public void InitiateInventory()
        {
            if (items.Count > 0) { handItem = items[0]; }
        }

        public void AddItem(BaseItemSO item)
        {
            item.AddToInventory(this);
            
            //OnItemAdded?.Invoke(item);
        }

        public void RemoveItem(BaseItemSO item)
        {
            int index = item.RemoveFromInventory(this);
            OnItemRemoved?.Invoke(item, index);
        }

        public void SetHandItem(BaseItemSO item)
        {
            if (handItem == item) return;
            item.SetAsHandItem(this);
        }

        public void InvokeOnItenAddedEvent(BaseItemSO item)
        {
            OnItemAdded?.Invoke(item);
        }
    }
}

