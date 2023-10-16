using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Items/FlashLightSO")]
    public class FlashLightSO : SingleItemSO
    {
        public float currentCharge = 10f;

        public SimpleLoopTimerSO chargeTimer;

        public AnimatorSO animator;

        public override void SetAsHandItem(InventorySO inventory)
        {
            inventory.handItem.OnUnregisterHandItem();
            inventory.handItem = this;
            inventory.handItem.OnRegisterHandItem();
        }

        public override void OnRegisterHandItem()
        {
            if (action == null) { Debug.Log("null"); }
            action.canceled += ActionHanlder;
            action.Enable();
            animator.SetAnimatorParameter("isHolding", true);
        }

        public override void OnUnregisterHandItem()
        {
            action.canceled -= ActionHanlder;
            action.Disable();
            animator.SetAnimatorParameter("isHolding", false);
            //SetDefault();
            //gameObjectReference.GetComponent<BaseItemMono>().SetInventoryPosition();
        }

        public void ActionHanlder(InputAction.CallbackContext context)
        {
            ControlFlashLight();
        }

        public void ControlFlashLight()
        {
            //need a spawner
            //var light = prefab.gameObject....
            var light = FindObjectOfType<BaseItemMono>().gameObject.GetComponentInChildren<LightController>();
            light.OpenAndClose();
        }

        public void OpenAndClose(bool key)
        {
            LightController light = FindObjectOfType<BaseItemMono>().gameObject.GetComponentInChildren<LightController>();
            light.OpenAndClose(key);
        }

        public override void SetDefault()
        {
            LightController light = FindObjectOfType<BaseItemMono>().gameObject.GetComponentInChildren<LightController>();
            light.OpenAndClose(false);
        }

        public override void WriteSpecial(SaveDataWriter writer)
        {
            writer.Write(currentCharge);
        }

        public override void ReadSpecial(SaveDataReader reader)
        {
            reader.ReadFloat();
        }
    }

}

