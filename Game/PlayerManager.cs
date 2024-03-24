using UnityEngine;
using GMEngine.Value;
using Cysharp.Threading.Tasks;

namespace GMEngine.Game
{
    public class PlayerManager : Singleton<PlayerManager>, IGameDataSender, ISaveDataRecevier
    {
        public GameObject playerPrefab;

        public InventorySO playerInventorySO;
        public BaseItemSO FallbackItemSO;

        public FloatReferenceRO playerMaxHealth;
        public FloatReferenceRO playerMaxMental;

        public FloatReferenceRW playerCurrentMental;
        public FloatReferenceRW playerCurrentHealth;

        protected override void OnAwake()
        {
            InitializePlayer();
        }

        private void OnEnable()
        {
            RegisterStroage();
        }

        public void InitializePlayer()
        {
            Debug.Log("Init Player");

#if UNITY_EDITOR
            if (GameObject.FindWithTag("MainChara") != null)
            {
                SetupStatus();
                SetupBasicInventory();
                return;
            }
#endif
            Instantiate(playerPrefab);
            SetupStatus();
            SetupBasicInventory();
        }

        public static GameObject GetPlayer()
        {
            return GameObject.FindGameObjectWithTag("MainChara");
        }

        private void SetupBasicInventory()
        {
            playerInventorySO.items.Clear();
            var player = GameObject.FindGameObjectWithTag("MainChara");
            InventoryController controller = player.GetComponent<InventoryController>();
            controller.AddFallbackItem();
            controller.SetupFallBackHandItem();
        }

        private void SetupStatus()
        {
            playerCurrentHealth.WriteValue(playerMaxHealth.Value);
            playerCurrentMental.WriteValue(playerMaxMental.Value);
        }

        private void RegisterStroage()
        {
            SimpleStroage stroage = GameManager.Instance.GetComponent<SimpleStroage>();
            stroage.RegisterSendEvtListener(this).Forget();
            stroage.RegisterReceiveEvtListener(this).Forget();
        }

        public void SendData(SaveData data)
        {
            var playerReference = GameObject.FindGameObjectWithTag("MainChara");

            data.playerPosition = playerReference.transform.position;
            data.playerRotation = playerReference.transform.rotation;

            data.playerHealth = playerCurrentHealth.Value;
            data.playerMental = playerCurrentMental.Value;

            //the first(number0)item is hand, which is a fall back item, so we should skip it

            //for (int i = 1; i < playerInventorySO.items.Count; i++)
            //{
            //    data.inventoryItemDatas.Add(data.PackToSaveData(playerInventorySO.items[i].gameObjectReference));
            //}

            //if (playerInventorySO.items.Contains(playerInventorySO.handItem))
            //{
            //    data.handItemNum = playerInventorySO.items.IndexOf(playerInventorySO.handItem);
            //    Debug.Log($"Current handitem index  {data.handItemNum}");
            //}
            //else
            //{
            //    data.handItemNum = -1;
            //}
        }

        public async UniTask ReceiveData(SaveData data)
        {
            Debug.Log("Receiving Player Data...");
            //disable some component for receiving data...
            var playerReference = GameObject.FindGameObjectWithTag("MainChara");

            playerReference.GetComponent<Animator>().enabled = false;
            playerReference.GetComponent<CharacterController>().enabled = false;

            playerReference.transform.position = data.playerPosition;
            playerReference.transform.rotation = data.playerRotation;

            SetupStatus();
            SetupBasicInventory();

            foreach (var item in data.inventoryItemDatas)
            {
                Vector3 inventoryPosition = new Vector3(0, -5, 0);

                Debug.Log($"{name} trying to create {item.itemName}");
                GameObject go = await ItemManager.Instance.CreateItemAsync(item.itemName, inventoryPosition, Quaternion.identity);
                go.GetComponent<InventoryItem>().baseItemSO.GetSOData(item.itemDataBuffer);
                go.GetComponent<InventoryItem>().SetPickDectecCollider(false);
                playerInventorySO.AddItem(go.GetComponent<InventoryItem>().baseItemSO);
            }

            if (data.handItemNum != -1)
            {
                BaseItemSO handItemSO = playerInventorySO.items[data.handItemNum];
                Debug.Log(handItemSO.name);
                //playerReference.GetComponentInChildren<PlayerKnowledge>().SetSelectingItem(handItemSO.gameObjectReference);
                //playerInventorySO.SetHandItem(handItemSO);
            }


            //enable
            playerReference.GetComponent<Animator>().enabled = true;
            playerReference.GetComponent<CharacterController>().enabled = true;

        }

    }

}
