using UnityEngine;

namespace GMEngine.Game
{
    public class ItemMenu : MonoBehaviour
    {
        public ItemSlot itemSlot;
        public void EquipItem()
        {
            if(itemSlot != null)
            {
                var controller = GameObject.FindGameObjectWithTag("MainChara").GetComponent<Brain>();
                controller.EquipItem(itemSlot.item);
                gameObject.SetActive(false);
            }
        }

        public void DropItem()
        {
            if (itemSlot != null)
            {
                if(itemSlot.item.baseItemSO is BareHandSO) { return; }
                Debug.Log("dropping item..");
                GameObject.FindGameObjectWithTag("MainChara").GetComponent<Brain>().DropItem(itemSlot.item, true);
                gameObject.SetActive(false);
            }

        }

        private void OnDisable()
        {
            itemSlot = null;
        }
    }

}

