using UnityEngine;
using UnityEngine.InputSystem;
using System;
using GMEngine.Value;

namespace GMEngine
{
    public class Brain : MonoBehaviour
    {
        [Header("Input")]
        //change into InputSO or Standard InputAsset later
        public InputAction pickEquipAction;
        public InputAction dropAction;

        [Header("Action Timer")]
        public SimpleTimerSO pickEquipTimer;

        [Header("Animation")]
        public AnimatorSO animator;
        [SerializeField]
        public Transform rightHandItemSlot;

        [Header("Events")]
        //events for monobehaviour
        //so event
        public GameActionDelegate OnPickingItem;

        public FloatReferenceRW pickProcessValue;
        //c# event
        public event EventHandler<Transform> OnEquipItem;

        [Header("Inventory and Knowledge")]
        //we need to store the konwledge for ui usage
        public PlayerKnowledge knowledge;
        public InventorySO inventory;

        private void OnEnable()
        {
            pickEquipAction.Enable(); 
            pickEquipAction.performed += PickEquipItemAction;
            pickEquipAction.canceled += PickEquipItemAction;

            dropAction.Enable(); dropAction.canceled += DropItemAction;
        }

        private void OnDisable()
        {
            pickEquipAction.Disable();
            pickEquipAction.performed -= PickEquipItemAction;
            pickEquipAction.canceled -= PickEquipItemAction;

            dropAction.Disable(); dropAction.canceled -= DropItemAction;
        }

        public async void PickEquipItemAction(InputAction.CallbackContext context)
        {
            if (knowledge.haveGrabbleItem())
            {

                if (context.performed)
                {
                    // Hold key long enough to pick and equip item
                    bool timerCompleted = await pickEquipTimer.StartCountAsync();
                    if (timerCompleted)
                    {
                        PickAndEquip();
                    }
                }
                else if (context.canceled)
                {
                    PickItem();
                }

                pickEquipTimer.ResetTimer();
            }
        }
        public void PickItem()
        {
            if (!knowledge.haveGrabbleItem()) return;
            inventory.AddItem(knowledge.grabbleItem.GetComponent<BaseItemMono>().baseItemSO);
            OnPickingItem?.Raise();
        }

        public void EquipItem()
        {
            //play the animation and will trigger the animation event
            inventory.SetHandItem(knowledge.GetSelectingItem().GetComponent<BaseItemMono>().baseItemSO);

            OnEquipItem?.Invoke(this, transform);
        }

        public void PickAndEquip()
        {
            //pick
            PickItem();

            //transfer Item State
            knowledge.SetSelectingItem(knowledge.grabbleItem);
            knowledge.grabbleItem = null;

            //equip
            EquipItem();
        }

        public void DropItemAction(InputAction.CallbackContext context)
        {
            if(inventory.items.Count == 1 || inventory.handItem is BareHandSO)
            {
                return;
            }

            DropItem(inventory.handItem.gameObjectReference);
        }

        public void DropItem(GameObject itemToDrop)
        {
            knowledge.SetSelectingItem(itemToDrop);
            inventory.SetHandItem(inventory.items[0]);
            animator.SetAnimatorParameter("OnDrop");
            inventory.RemoveItem(itemToDrop.GetComponent<BaseItemMono>().baseItemSO);
        }

        /// <summary>
        /// Animation Event Handler
        /// </summary>
        public void EquipItemHandler()
        {
            GameObject itemToEquip = knowledge.GetSelectingItem();
            itemToEquip.transform.SetParent(rightHandItemSlot);
            itemToEquip.transform.localPosition = Vector3.zero;
            itemToEquip.transform.rotation = itemToEquip.transform.root.rotation;
        }

        /// <summary>
        /// Animation Event Handler
        /// </summary>
        public void ThrowItemHandler()
        {
            GameObject itemToDrop = knowledge.GetSelectingItem();
            itemToDrop.GetComponent<BaseItemMono>().DropToGround();
        }
    }
}
