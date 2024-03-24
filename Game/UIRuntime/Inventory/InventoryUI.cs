using Cysharp.Threading.Tasks;
using GMEngine.GameObjectExtension;
using UnityEngine;
using GMEngine.UI;

namespace GMEngine.Game
{
    public class InventoryUI : GMUgui
    {
        [Header("Items and Knowledges")]
        public InventoryController inventory;
        public PlayerKnowledge knowledge;

        [Header("Slot Factory")]
        public GameObjectFactory factory;

        [Header("Slot Cache")]
        public GameObject itemSlotPrefab;
        public Transform itemSlots;
        public GameObject itemMenu;

        [Header("Inventory Display Setting")]
        public int maxColumn = 4;
        public int maxRow;
        public float columnInterval = 45f;
        public float rowInterval = 50f;

        [Header("Audio")]
        public AudioSourceSO audioSourceSO;
        public AudioSO openAudioSO;

        [Header("Debug")]
        public int slotCount = 0;

        public override async void Initiate()
        {
            //the initialization be incharged by ui manager
            await InitiateInventoryUI();
        }

        private void OnDestroy()
        {
            inventory.OnItemAdded -= AppendItemSlot;
            inventory.OnItemRemoved -= RemoveItemSlot;
        }

        public async UniTask InitiateInventoryUI()
        {
            Debug.Log("initiate inventory ui...");
            GameObject chara = await gameObject.AwaitFindGameObjectWithTag("MainChara");
            Debug.Log(chara.name);

            knowledge = chara.GetComponentInChildren<PlayerKnowledge>();

            inventory = chara.GetComponent<InventoryController>();
            inventory.OnItemAdded += AppendItemSlot;
            inventory.OnItemRemoved += RemoveItemSlot;

            InitiateSlotFactory();

            slotCount = 0;

            FillInventoryUI(inventory);

        }

        private void InitiateSlotFactory()
        {
            if (factory != null) return;
            factory = FindAnyObjectByType<GameObjectFactory>();
            if (factory == null || factory.gameObjectPrefab != itemSlotPrefab)
            {
                factory = ScriptableObject.CreateInstance<GameObjectFactory>();
                factory.name = itemSlotPrefab.name;
            }
            factory.Setup(itemSlotPrefab, maxRow * maxColumn);
        }

        private void FillInventoryUI(InventoryController inventory)
        {
            itemSlots = transform.Find("ItemSlots");

            if (itemSlots.transform.childCount > 0)
            {
                foreach (Transform children in itemSlots.transform)
                {
                    factory.PoolProduct(children.gameObject);
                }
            }

            foreach (var item in inventory.Items)
            {
                AppendItemSlot(slotCount++, item);
            }
        }

        public void AppendItemSlot(int number, InventoryItem item)
        {
            ItemSlot itemSlot = GetItemSlot(item);
            SetUpSlotPosition(itemSlot, number);
            itemSlot.SetupItem(item);
        }

        public void AppendItemSlot(InventoryItem item)
        {
            int number = inventory.InventorySO.items.Count - 1;
            ItemSlot itemSlot = GetItemSlot(item);
            SetUpSlotPosition(itemSlot, number);
            itemSlot.SetupItem(item);
        }

        private ItemSlot GetItemSlot(InventoryItem item)
        {
            ItemSlot itemSlot = factory.GetProduct(itemSlots).GetComponent<ItemSlot>();
            return itemSlot;
        }

        private void SetUpSlotPosition(ItemSlot itemSlot, int number)
        {
            Vector2 position = AllocSlotPosition(number);
            itemSlot.transform.localPosition = position;
        }

        private void RemoveItemSlot(InventoryItem item, int index)
        {
            Debug.Log($"Begin removing item {item.name}");
            foreach (Transform child in itemSlots)
            {
                ItemSlot slot = child.GetComponent<ItemSlot>();
                if (slot.item.gameObject == item.gameObject)
                {
                    Debug.Log($"found item, now pooling, item is {slot.item.name}");
                    factory.PoolProduct(slot.gameObject);
                    break;
                }
            }
        }

        public Vector2 AllocSlotPosition(int number)
        {
            int x = number % maxColumn;
            int y = number / maxColumn;
            Vector2 position = new Vector2(x * columnInterval, -y * rowInterval);
            return position;
        }

        public int GetNumberFromPosition(Vector2 position)
        {
            int x = Mathf.FloorToInt(position.x / columnInterval);
            int y = Mathf.FloorToInt(-position.y / rowInterval);

            int number = y * maxColumn + x;

            return number;
        }
        public void CallItemMenu(Vector2 mousePosition, ItemSlot itemSlot)
        {
            itemMenu.transform.position = mousePosition;
            itemMenu.GetComponent<ItemMenu>().itemSlot = itemSlot;
            itemMenu.gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        [Header("Editor")]
        public float widthOffset;
        public float heightOffset;

        private void OnValidate()
        {
            enabled = false;
            enabled = true;
        }
#endif
    }
}

