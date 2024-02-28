using UnityEngine;

namespace GMEngine
{
    public class ItemMenu : MonoBehaviour
    {
        public ItemSlot itemSlot;
        public void EquipItem()
        {
            if(itemSlot != null)
            {
                var controller = GetComponentInParent<InventoryUI>();
                controller.knowledge.SetSelectingItem(itemSlot.itemSOSlot.gameObjectReference);
                controller.inventorySO.SetHandItem(itemSlot.itemSOSlot);
                gameObject.SetActive(false);
            }
        }

        public void DropItem()
        {
            if (itemSlot != null)
            {
                if(itemSlot.itemSOSlot is BareHandSO) { return; }
                Debug.Log("dropping item..");
                GameObject.FindGameObjectWithTag("MainChara").GetComponent<Brain>().DropItem(itemSlot.itemSOSlot.gameObjectReference);
                gameObject.SetActive(false);
            }

        }

        private void OnDisable()
        {
            itemSlot = null;
        }
    }

}

