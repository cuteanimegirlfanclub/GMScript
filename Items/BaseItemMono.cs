using UnityEngine;
using System;

namespace GMEngine
{
    [DisallowMultipleComponent]
    public class BaseItemMono : MonoBehaviour, ISaveDataSender
    {
        [Header("Item Data")]
        public BaseItemSO baseItemSO;

        [Header("Item Audio Clips")]
        public AudioSO pickAudio;
        public AudioSO equipAudio;
        public AudioSO throwAudio;

        [Header("Item Audio Source")]
        public AudioSourceSO ItemAudioSource;

        [Header("Debug")]
        public bool isGrabbleItem = false;

        private void OnEnable()
        {
            SimpleStroage stroage = GameManager.Instance.GetComponent<SimpleStroage>();
            stroage.OnPullDataRequest += SendData;
        }

        private void OnDisable()
        {
            SimpleStroage stroage = GameManager.Instance.GetComponent<SimpleStroage>();
            stroage.OnPullDataRequest -= SendData;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Brain brain))
            {
                //check if the picker facing the item
                Vector3 itemToKonwledge = transform.position - brain.transform.position;
                if (Vector3.Dot(itemToKonwledge, brain.transform.forward) > 0)
                {
                    //then set the item grabble
                    brain.knowledge.SetGrabbleItem(gameObject);

                    brain.OnPickingItem.Subscribe(OnPick);
                    brain.OnEquipItem += OnEquip;

                    isGrabbleItem = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isGrabbleItem)
            {
                other.TryGetComponent(out Brain brain);
                brain.knowledge.grabbleItem = null;
                //brain.knowledge.selectingItemGO = null;


                brain.OnPickingItem.UnSubscribe(OnPick);
                brain.OnEquipItem -= OnEquip;

                isGrabbleItem = false;
            }
        }

        //Events when action
        //trying scriptableobject event
        public void OnPick()
        {
            //pickAudio.Play(ItemAudioSource.GetAudioSource());
            //baseItemSO.SetDefault();
            if(gameObject.transform.root != transform)
            {
                gameObject.transform.parent = null;
            }
            SetInventoryPosition();
        }

        public void SetInventoryPosition()
        {
            gameObject.transform.position = new Vector3(0, -5, 0);
        }

        //trying c# event
        private void OnEquip(object sender, Transform holderTransform)
        {
            GetComponent<BoxCollider>().enabled = false;
            isGrabbleItem = false;
        }


        public void ThrowItem()
        {
            GetComponent<BoxCollider>().enabled = true;
            transform.parent = null;
            Mathf.MoveTowards(transform.position.y, 0, Time.deltaTime);
        }

        public void DropToGround()
        {
            Vector3 position = transform.position;
            Quaternion rotaion;

            rotaion = GameObject.FindGameObjectWithTag("MainChara").transform.rotation;

            transform.parent = null;
            transform.position = new Vector3(position.x, 0.02f, position.z);
            transform.rotation = rotaion;

            var collider = GetComponent<BoxCollider>();
            if (!collider.enabled) { collider.enabled = true; }
        }

        public void SendData(SaveData data)
        {
            int state = StateCheck();
            if(state == 1)
            {
                Debug.Log("Sending ground item data...");
                data.groundItemDatas.Add(data.PackToSaveData(gameObject));
            }
        }

        /// <summary>
        /// 0 -> InInventory; 1 -> OnGround;  2 -> OnHand
        /// </summary>
        /// <returns></returns>
        private int StateCheck()
        {
            if (transform.position.y < 0) return 0;
            else if (transform.parent != null) return 2; else return 1;
        }
    }

}
