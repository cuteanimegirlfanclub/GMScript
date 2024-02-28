using GMEngine.Value;
using System;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Items/FlashLightSO")]
    public class FlashLightSO : SingleItemSO
    {
        public FloatVariable currentCharge;

        public override void SetAsHandItem(InventorySO inventory)
        {
            inventory.handItem.OnUnregisterHandItem(inventory);
            inventory.handItem = this;
            inventory.handItem.OnRegisterHandItem(inventory);
        }

        public override void OnRegisterHandItem(InventorySO inventory)
        {
            //if (action == null) { Debug.Log("null"); }
            //action.canceled += ActionHanlder;
            //action.Enable();
            inventory.animator.SetBool("isHolding", true);

            if(gameObjectReference != null)
            {
                gameObjectReference.GetComponent<ItemBehaviour>().enabled = true;
            }
        }

        public override void OnUnregisterHandItem(InventorySO inventory)
        {
            //action.canceled -= ActionHanlder;
            //action.Disable();
            //SetDefault();
            //gameObjectReference.GetComponent<BaseItemMono>().SetInventoryPosition();

            inventory.animator.SetBool("isHolding", false);
            if (gameObjectReference != null)
            {
                gameObjectReference.GetComponent<ItemBehaviour>().enabled = false;
            }
        }

        public override void SetDefault()
        {
            LightController light = gameObjectReference.GetComponentInChildren<LightController>();
            light.OpenAndClose(false);
        }

        public override byte[] BufferSOData()
        {
            return BitConverter.GetBytes(currentCharge.Value);
        }

        public override void GetSOData(byte[] buffer)
        {
            currentCharge.SetValue(BitConverter.ToSingle(buffer, 0));
        }
    }

}

