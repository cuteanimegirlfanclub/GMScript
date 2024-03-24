using UnityEngine;
using UnityEngine.InputSystem;
using System;
using GMEngine.Value;
using GMEngine.UI;
using UnityEngine.InputSystem.Interactions;
using GMEngine.TransformExtension;
using UnityEngine.Animations.Rigging;

namespace GMEngine.Game
{
    public class Brain : MonoBehaviour
    {
        [Header("Input")]
        //change into InputSO or Standard InputAsset later
        public InputAction pickEquipAction;
        public InputAction dropAction;
        public DialogueDisplayer displayer;

        [Header("Action Timer Setting")]
        [Obsolete] private SimpleTimerSO pickEquipTimer;
        public TimerSliderController timerControl;
        public FloatReferenceRO duration;

        [Header("Animation")]
        //public AnimatorSO animator;
        public Animator animator;
        [SerializeField]
        public Transform rightHandItemSlot;
        //public FloatReferenceRW pickProcessValue;

        [Header("Inventory")]
        public InventoryController inventory;
        [Header("Memory")]
        public PlayerKnowledge knowledge;


        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            pickEquipAction.Enable();
            pickEquipAction.started += ShowRoundSliderAndBeginPickActionSequence;

            dropAction.Enable();
            dropAction.canceled += DropHandItemAction;

            displayer.DisplayDialogue("trying to use the dialogue displayer to show dialogue").Forget();
        }

        private void OnDisable()
        {
            pickEquipAction.Disable();
            pickEquipAction.started -= ShowRoundSliderAndBeginPickActionSequence;

            dropAction.Disable();
            dropAction.canceled -= DropHandItemAction;
        }


        private void ShowRoundSliderAndBeginPickActionSequence(InputAction.CallbackContext context)
        {
            if (!knowledge.haveGrabbleItem()) return;

            if(context.interaction is HoldInteraction holdInteraction)
            {
                holdInteraction.duration = duration.Value;
                timerControl.UseSliderOnce(duration.Value);

                pickEquipAction.performed += PickEquipItemAction;
                pickEquipAction.canceled += PickItemAction;
            }
            else
            {
                Debug.LogError($"{gameObject.name}'s {pickEquipAction.name} need to have Hold Interaction!");
            }
            Debug.Log(context.phase);
        }

        private void PickEquipItemAction(InputAction.CallbackContext context)
        {
            timerControl.HideSlider();
            //Debug.Log(context.phase);
            pickEquipAction.canceled -= PickItemAction;
            PickAndEquip(knowledge.GrabbleItem);
        }

        private void PickItemAction(InputAction.CallbackContext context)
        {
            timerControl.HideSlider();
            //Debug.Log(context.phase);
            pickEquipAction.performed -= PickEquipItemAction;
            PickItem(knowledge.GrabbleItem);
        }

        public void PickItem(InventoryItem item)
        {
            inventory.AddItem(item);
            //animator.Play();
            knowledge.SetGrabbleItem(null);
        }

        public void PickItem()
        {
            if (!knowledge.haveGrabbleItem())
            {
                return;
            }
            PickItem(knowledge.GrabbleItem);
        }

        public void EquipItem(InventoryItem item)
        {
            inventory.ItemOnHand = item;
        }

        public void PickAndEquip(InventoryItem item)
        {
            //pick
            PickItem(item);
            //equip
            EquipItem(item);
        }

        private void DropHandItemAction(InputAction.CallbackContext context)
        {
            DropHandItem(true);
        }

        public void DropHandItem(bool playAnimation)
        {
            DropItem(inventory.ItemOnHand, playAnimation);
        }

        public void DropItem(InventoryItem item,bool playAnimation)
        {
            if (item == null || item.baseItemSO is BareHandSO) return;
            inventory.GetComponent<Animator>().SetBool(inventory.ItemOnHand.onEquipStateInfo, false);
            if (playAnimation)
            {
                animator.SetBool("OnDrop", true);
                knowledge.SetSelectingItem(item);
            }
            else
            {
                throw new NotImplementedException();
            }
            inventory.SetupFallBackHandItem();
        }
        /// <summary>
        /// Animation Event Handler
        /// </summary>
        public void EquipItemHandler()
        {
            GameObject itemToEquip = inventory.ItemOnHand.gameObject;
            itemToEquip.transform.SetParent(rightHandItemSlot);
            itemToEquip.transform.localPosition = Vector3.zero;
            itemToEquip.transform.rotation = itemToEquip.transform.root.rotation;
        }

        /// <summary>
        /// Animation Event Handler
        /// </summary>
        public void ThrowItemHandler()
        {
            knowledge.SelectingItem.DropToGround(inventory);
            knowledge.SetSelectingItem(null);
        }
    }
}
