using UnityEngine;
using Unity;
using GMEngine.GameObjectExtension;
using GMEngine.TransformExtension;
using UnityEngine.Events;

namespace GMEngine.Game
{
    [DisallowMultipleComponent, RequireComponent(typeof(BoxCollider))]
    public class InventoryItem : MonoBehaviour
    {
        [Header("Item Data")]
        public BaseItemSO baseItemSO;

        [Header("Item Audio Clips")]
        public AudioSO pickAudio;
        public AudioSO equipAudio;
        public AudioSO throwAudio;

        [Header("Debug")]
        public bool isGrabbleItem = false;

        [Header("Animation Requirement")]
        public string onEquipStateInfo;

        [Header("Event")]
        public UnityEvent setDefaultState;

        public UnityAction equipAction;
        public UnityAction dropAction;

        public void Awake()
        {
            baseItemSO = baseItemSO.DeepCopy();
            equipAction += EquipThisItem;
            dropAction += DropThisItem;
        }

        private void OnTriggerStay(Collider other)
        {
            if (isGrabbleItem) return;
            if(other.TryGetComponent(out Brain brain))
            {
                //check if the picker facing the item
                Vector3 itemToKonwledge = transform.position - brain.transform.position;
                if (Vector3.Dot(itemToKonwledge, brain.transform.forward) > 0)
                {
                    //then set the item grabble
                    brain.knowledge.SetGrabbleItem(this);
                    isGrabbleItem = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isGrabbleItem)
            {
                other.TryGetComponent(out Brain brain);
                brain.knowledge.SetGrabbleItem(null);
                isGrabbleItem = false;
            }
        }

        public void OnRegisterHandItem(InventoryController inventory)
        {
            GetComponent<BoxCollider>().enabled = false;
            isGrabbleItem = false;
            inventory.GetComponent<Animator>().SetBool(onEquipStateInfo, true);
            TrySetItemBehaviour(true);
        }

        public void OnUnregisterHandItem(InventoryController inventory)
        {
            inventory.GetComponent<Animator>().SetBool(onEquipStateInfo, false);
            setDefaultState?.Invoke();
            OnEnterInventory();
        }

        public void OnEnterInventory()
        {
            //pickAudio.Play(ItemAudioSource.GetAudioSource());
            //baseItemSO.SetDefault();
            SetInventoryPosition();
            TrySetItemBehaviour(false);
        }

        public void SetInventoryPosition()
        {
            if (baseItemSO is BareHandSO) return;
            if (gameObject.transform.root != transform)
            {
                gameObject.transform.root.parent = null;
            }
            gameObject.transform.position = new Vector3(0, -5, 0);
            Debug.Log("setting inventory position");
        }
        private void TrySetItemBehaviour(bool key)
        {
            if (TryGetComponent(out ItemBehaviour behaviourComponent))
            {
                behaviourComponent.enabled = key;
            }
        }

        public void SetPickDectecCollider(bool key)
        {
            GetComponent<BoxCollider>().enabled = key;
        }

        public void DropToGround(InventoryController inventory)
        {
            Vector3 position = inventory.GetComponent<Brain>().rightHandItemSlot.position;
            Quaternion rotaion = inventory.transform.rotation;
            transform.parent = null;
            transform.position = new Vector3(position.x, 0.02f, position.z);
            transform.rotation = rotaion;

            SetPickDectecCollider(true);
            inventory.RemoveItem(this);
        }

        private void EquipThisItem()
        {
            PlayerManager.GetPlayer().GetComponent<Brain>().EquipItem(this);
        }

        private void DropThisItem()
        {
            PlayerManager.GetPlayer().GetComponent<Brain>().DropItem(this, true);
        }
    }

}
