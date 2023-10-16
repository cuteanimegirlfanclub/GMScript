using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GMEngine
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IDragHandler
    {
        [SerializeField]
        private Image spriteSlot;
        public BaseItemSO itemSOSlot;

        public void SetupItem(BaseItemSO itemSO)
        {
            Debug.Log($"Get {itemSO}, Setting up {name}");
            itemSOSlot = itemSO;
            SetupSprite(itemSO.sprite);
            SetupItemCount(itemSO);
            Debug.Log($"{itemSOSlot.name}");
        }

        private void SetupItemCount(BaseItemSO itemSO) 
        {
            if (itemSO is StackableItemSO stackableItemSO)
            {
                //count.text = stackableItemSO.number.Value;
                //itemCountGO.gameObject.activeSelf(true);
            }
        }

        private void SetupSprite(Sprite sprite)
        {
            if (sprite != null)
            {
                if(spriteSlot == null)
                {
                    spriteSlot = transform.Find("ItemImage").GetComponent<Image>();    
                }
                spriteSlot.sprite = sprite;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("drag");
            var controller = GetComponentInParent<InventoryUI>();
            controller.itemMenu.gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("begin");
            foreach (var gameObject in eventData.hovered)
            {
                if(gameObject.TryGetComponent(out ItemSlot targetItemSlot))
                {
                    Debug.Log("exchange");
                    ExchangeItemSlot(targetItemSlot);
                    return;
                }
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            var controller = GetComponentInParent<InventoryUI>();
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                controller.CallItemMenu(eventData.position, this);
            }
            else
            {
                controller.itemMenu.gameObject.SetActive(false);
            }
        }

        private void ExchangeItemSlot(ItemSlot targetItemSlot)
        {
            BaseItemSO temp = this.itemSOSlot;
            this.SetupItem(targetItemSlot.itemSOSlot);
            targetItemSlot.SetupItem(temp);
        }

        public void OnDisable()
        {
            //Reset();
        }

        

        public void FactoryReset()
        {
            Debug.Log($"Reseting {gameObject.name}");
            //spriteSlot = null;
            spriteSlot.sprite = null;
            itemSOSlot = null;
        }

    }
}

