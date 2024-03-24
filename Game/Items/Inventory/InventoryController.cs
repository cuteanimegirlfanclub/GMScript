using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

namespace GMEngine.Game
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventorySO inventorySO;
        public InventorySO InventorySO { get => inventorySO; }

        [SerializeField] private List<InventoryItem> items;
        public List<InventoryItem> Items { get => items; }
        [SerializeField] private InventoryItem itemOnHand;
        public InventoryItem ItemOnHand
        {
            get => itemOnHand;
            set
            {
                if (value == null || itemOnHand == value) return;
                itemOnHand?.OnUnregisterHandItem(this);
                Debug.Log($"{itemOnHand.name} unregistering");
                itemOnHand = value;
                Debug.Log($"{itemOnHand.name} registering");
                itemOnHand?.OnRegisterHandItem(this);
            }
        }

        /// <summary>
        /// invoke ui action when item added
        /// </summary>
        public event Action<InventoryItem> OnItemAdded;

        /// <summary>
        /// invoke ui action when item added
        /// </summary>
        public event Action<InventoryItem, int> OnItemRemoved;


        public void AddItem(InventoryItem item)
        {
            inventorySO.AddItem(item.baseItemSO);
            items.Add(item);
            item.OnEnterInventory();
            OnItemAdded?.Invoke(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            int index = inventorySO.RemoveItem(item.baseItemSO);
            items.Remove(item);
            OnItemRemoved?.Invoke(item, index);
        }

        public void AddFallbackItem()
        {
            if (items[0].baseItemSO is BareHandSO so)
            {
                inventorySO.AddItem(so);
            }
            else
            {
                throw new Exception($"You need to set the {inventorySO.name}'s first item as bare hand!");
            }
        }

        public void SetupFallBackHandItem()
        {
            if (items[0].baseItemSO is BareHandSO)
            {
                itemOnHand = items[0];
            }
        }
    }

}
