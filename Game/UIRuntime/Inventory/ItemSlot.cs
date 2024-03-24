using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GMEngine.Game
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IDragHandler
    {
        [SerializeField]
        private Image spriteSlot;
        public InventoryItem item;

        public void SetupItem(InventoryItem item)
        {
            this.item = item;
            var itemSO = item.baseItemSO;
            SetupSprite(itemSO.sprite);
            SetupItemCount(itemSO);
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
                if(item.TryGetComponent(out ItemBehaviour behaviour))
                {
                    GlobalUIManager.SetupContextualMenu(behaviour.actionName, behaviour.evtDelegate);
                }
                GlobalUIManager.SetupContextualMenu("Equip", item.equipAction);
                GlobalUIManager.SetupContextualMenu("Drop", item.dropAction);
                GlobalUIManager.UseContextualMenu(eventData);
            }
        }

        private void ExchangeItemSlot(ItemSlot targetItemSlot)
        {
            var temp = this.item;
            this.SetupItem(targetItemSlot.item);
            targetItemSlot.SetupItem(temp);
        }

        public void FactoryReset()
        {
            Debug.Log($"Reseting {gameObject.name}");
            //spriteSlot = null;
            spriteSlot.sprite = null;
            item = null;
        }
    }
}

