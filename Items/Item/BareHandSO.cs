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
        public override void OnRegisterHandItem()
        {
            Debug.Log("i am holding my hand");
        }

        public override void SetAsHandItem(InventorySO inventory)
        {
            inventory.handItem.OnUnregisterHandItem();
            inventory.handItem = this;
            inventory.handItem.OnRegisterHandItem();
        }

        public override void SetDefault()
        {

        }

        public override void OnUnregisterHandItem()
        {

        }

        public override void WriteSpecial(SaveDataWriter writer)
        {

        }

        public override void ReadSpecial(SaveDataReader reader)
        {

        }
    }
}
