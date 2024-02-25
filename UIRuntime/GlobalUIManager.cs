using GMEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace GMEngine
{
    public class GlobalUIManager : Singleton<GlobalUIManager>
    {
        [Header("Input")]
        private readonly string inputReferenceName = "PlayerControl[UIControl/OpenInventory]";
        public InputActionReference openInventoryAction;

        private GameObject inventoryUIGO;
        private GameObject systemMessageBox;
        private GameObject loadingUI;

        [SerializeField]
        private PrefabDatabase prefabDatabase;
        private readonly string databaseName = "Global UI Prefab Database";

        protected override async void OnAwake()
        {
            await InitiateGlobalUI();

            InstantiateGlobalUIElement();

            await SetupGlobalUIController();
        }

        private void OnDisable()
        {
            openInventoryAction.action.Disable(); openInventoryAction.action.canceled -= DisplayInventoryUI;
        }

        private async UniTask InitiateGlobalUI()
        {
            AsyncOperationHandle<PrefabDatabase> handle = Addressables.LoadAssetAsync<PrefabDatabase>(databaseName);
            await handle.ToUniTask();
            if (handle.IsDone)
            {
                prefabDatabase = handle.Result;
            }
            else
            {
                Debug.LogError($"{gameObject.name} Initaite Error!");
                Application.Quit();
            }

        }

        private void InstantiateGlobalUIElement()
        {
            inventoryUIGO = Instantiate(prefabDatabase.GetPrefabAtIndex(0), transform);

            systemMessageBox = Instantiate(prefabDatabase.GetPrefabAtIndex(1), transform);

            loadingUI = Instantiate(prefabDatabase.GetPrefabAtIndex(2), transform);
        }

        private async UniTask SetupGlobalUIController()
        {
            AsyncOperationHandle<InputActionReference> input = Addressables.LoadAssetAsync<InputActionReference>(inputReferenceName);
            await input.ToUniTask();
            if (input.IsDone)
            {
                openInventoryAction = input.Result;
            }
            else
            {
                Debug.LogError($"{gameObject.name} Initaite Error!");
                Application.Quit();
            }

            //Bind Behaviour
            openInventoryAction.action.Enable(); openInventoryAction.action.canceled += DisplayInventoryUI;
        }
        

        public SuspendedConfirmationBox GetSystemBox()
        {
            return systemMessageBox.GetComponent<SuspendedConfirmationBox>();
        }


        public void SetLoadingUI(bool key)
        {
            Debug.Log($"Seting Loading UI To {key}");
            loadingUI.SetActive(key);
        }

        private void DisplayInventoryUI(InputAction.CallbackContext ctx)
        {
            inventoryUIGO.SetActive(!inventoryUIGO.activeSelf);
        }

        //public void InstantiateInventoryUI()
        //{
        //    inventoryUIGO = Instantiate(Resources.Load("InventoryUI", typeof(GameObject)), transform) as GameObject;
        //    var inventoryUI = inventoryUIGO.GetComponent<InventoryUI>();
        //    inventoryUI.InitiateInventoryUI(inventoryUI.inventorySO);
        //    inventoryUIGO.SetActive(false);
        //}

        //public void InstantiateSystemUIBox()
        //{
        //    systemMessageBox = Instantiate(Resources.Load("SuspendMessageBox", typeof(GameObject)), transform) as GameObject;
        //}
        //private void InstantiateLoadingUI()
        //{
        //    loadingUI = Instantiate(Resources.Load("LoadingUI", typeof(GameObject)), transform) as GameObject;
        //}
    }

}

