using GMEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Events;
using GMEngine.Game;
using UnityEngine.EventSystems;

namespace GMEngine
{
    public class GlobalUIManager : Singleton<GlobalUIManager>
    {
        private readonly string inputReferenceName = "PlayerControl[UIControl/OpenInventory]";
        private InputActionReference openInventoryAction;

        private GameObject inventoryUIGO;
        private GameObject systemMessageBox;
        private GameObject loadingUI;

        private GameObject globalContextualMenu;

        [SerializeField]
        private PrefabDatabase prefabDatabase;
        private readonly string databaseName = "Global UI Prefab Database";

        protected override async void OnAwake()
        {
            await InitiateGlobalUI();

            InstantiateGlobalUIElement();

            await SetupGlobalUIController();
        }

#if UNITY_EDITOR
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.P)) 
            {
                Debug.Break();
            }
        }
#endif

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
            Instance.inventoryUIGO = InstantiateUGUI(0);

            Instance.systemMessageBox = Instantiate(prefabDatabase.GetPrefabAtIndex(1), transform);

            Instance.loadingUI = Instantiate(prefabDatabase.GetPrefabAtIndex(2), transform);

            Instance.globalContextualMenu = InstantiateUGUI(3);
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
        
        public static GameObject InstantiateUGUI(int index)
        {
            var go = Instantiate(Instance.prefabDatabase.GetPrefabAtIndex(index), Instance.transform);
            Debug.Log($"Initiating {go.name}");
            go.GetComponent<GMUgui>().Initiate();
            return go;
        }

        public static SuspendedConfirmationBox GetSystemBox()
        {
            return Instance.systemMessageBox.GetComponent<SuspendedConfirmationBox>();
        }

        public static void SetLoadingUI(bool key)
        {
            Debug.Log($"Seting Loading UI To {key}");
            Instance.loadingUI.SetActive(key);
        }

        private void DisplayInventoryUI(InputAction.CallbackContext ctx)
        {
            inventoryUIGO.SetActive(!inventoryUIGO.activeSelf);
        }

        public static void SetupContextualMenu(string actionName, UnityAction action)
        {
            GlobalContextualMenu menu = Instance.globalContextualMenu.GetComponent<GlobalContextualMenu>();
            menu.AppendAction(actionName, action);
        }

        public static void UseContextualMenu(PointerEventData eventData)
        {
            GlobalContextualMenu menu = Instance.globalContextualMenu.GetComponent<GlobalContextualMenu>();
            if (menu.gameObject.activeSelf) return;
            if (!menu.isSetup) return;
            menu.GetComponent<RectTransform>().position = eventData.position;
            menu.gameObject.SetActive(true);
        }
    }

}

