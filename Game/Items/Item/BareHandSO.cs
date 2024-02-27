using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Items/BareHandSO")]
    public class BareHandSO : SingleItemSO
    {
        public override void SetAsHandItem(InventorySO inventory)
        {
            inventory.handItem.OnUnregisterHandItem(inventory);
            inventory.handItem = this;
            inventory.handItem.OnRegisterHandItem(inventory);
        }
        public override void OnRegisterHandItem(InventorySO inventory)
        {
            Debug.Log("i am holding my hand");
        }

        public override void SetDefault()
        {

        }

        public override void OnUnregisterHandItem(InventorySO inventory)
        {

        }

        public override byte[] BufferSOData()
        {
            return new byte[0];
        }

        public override void GetSOData(byte[] buffer)
        {

        }
    }
}
