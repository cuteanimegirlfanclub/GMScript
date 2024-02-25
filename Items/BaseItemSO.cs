using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    public abstract class BaseItemSO : ScriptableObject, IPersistableSO
    {
        /// <summary>
        /// the gameObject will be reference when the BaseItemSO was loaded
        /// </summary>
        public GameObject gameObjectReference;

        /// <summary>
        /// will be repalce with InputActionReference
        /// </summary>
        public Sprite sprite;

        [TextArea(10,15)]
        public string description;

        /// <summary>
        /// ***Dont use this***
        /// </summary>
        /// <param name="inventory"></param>
        public abstract void AddToInventory(InventorySO inventory);

        /// <summary>
        /// ***Dont use this***
        /// </summary>
        /// <param name="inventory"></param>
        public abstract int RemoveFromInventory(InventorySO inventory); 
        public abstract void SetAsHandItem(InventorySO inventory);

        public abstract void SetDefault();
        /// <summary>
        /// Behaviours when the item was set as HandItem
        /// </summary>
        public abstract void OnRegisterHandItem(InventorySO inventory);
        /// <summary>
        /// Behaviours when the item was remove form HamdItem
        /// </summary>
        public abstract void OnUnregisterHandItem(InventorySO inventory);

        public abstract byte[] BufferSOData();
        public abstract void GetSOData(byte[] buffer);
    }

    public abstract class SingleItemSO : BaseItemSO
    {
        public override void AddToInventory(InventorySO inventory)
        {
            if (inventory.items.Contains(this))
            {
                return;
            }
            else
            {
                inventory.items.Add(this);
                inventory.InvokeOnItenAddedEvent(this);
            }
        }

        public override int RemoveFromInventory(InventorySO inventory)
        {
            if (inventory.items.Contains(this))
            {
                inventory.items.Remove(this);
                return inventory.items.IndexOf(this);
            }
            else
            {
                return -1;
            }
        }
    }

    public abstract class StackableItemSO : BaseItemSO
    {
        public float number;

        public override void AddToInventory(InventorySO inventory)
        {
            if (inventory.items.Contains(this))
            {
                StackableItemSO item = GetItem(inventory);
                item.number += 1f;
            }
            else
            {
                inventory.items.Add(this);
            }
        }

        public override int RemoveFromInventory(InventorySO inventory)
        {
            if (inventory.items.Contains(this))
            {
                StackableItemSO item = GetItem(inventory);
                item.number -= 1f;
                return inventory.items.IndexOf(this);
            }
            else
            {
                return -1;
            }
        }
        public override byte[] BufferSOData()
        {
            return BitConverter.GetBytes(number);
        }
        public override void GetSOData(byte[] buffer)
        {
            number = BitConverter.ToSingle(buffer, 0);
        }

        //get item from the inventory
        private StackableItemSO GetItem(InventorySO inventory)
        {
            StackableItemSO item = inventory.items.Find(item => item.Equals(this)) as StackableItemSO;
            return item;
        }
    }



}
