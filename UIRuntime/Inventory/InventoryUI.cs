using GMEngine.GameObjectExtension;
using UnityEngine;

namespace GMEngine
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("Items and Knowledges")]
        public InventorySO inventorySO;
        public PlayerKnowledge knowledgeSO;

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

        private void Awake()
        {
            inventorySO.OnItemAdded += AppendItemSlot;
            inventorySO.OnItemRemoved += RemoveItemSlot;
        }

        private void Start()
        {
            //the initialization should be after the player
            InitiateInventoryUI(inventorySO);
        }

        private void OnDestroy()
        {
            inventorySO.OnItemAdded -= AppendItemSlot;
            inventorySO.OnItemRemoved -= RemoveItemSlot;
        }

        public void InitiateInventoryUI(InventorySO inventory)
        {
            Debug.Log("initiate inventory ui...");

            InitiateSlotFactory();

            slotCount = 0;

            FillInventoryUI(inventory);

        }



        private void InitiateSlotFactory()
        {
            factory = FindAnyObjectByType<GameObjectFactory>();
            if (factory == null || factory.gameObjectPrefab != itemSlotPrefab)
            {
                factory = ScriptableObject.CreateInstance<GameObjectFactory>();
                factory.name = itemSlotPrefab.name;
            }
            factory.Setup(itemSlotPrefab, maxRow * maxColumn);
        }

        private void FillInventoryUI(InventorySO inventory)
        {
            itemSlots = transform.Find("ItemSlots");

            if (itemSlots.transform.childCount > 0)
            {
                foreach (Transform children in itemSlots.transform)
                {
                    factory.PoolProduct(children.gameObject);
                }
            }

            foreach (var item in inventory.items)
            {
                AppendItemSlot(slotCount++, item);
            }
        }


        public void AppendItemSlot(int number, BaseItemSO itemSO)
        {
            ItemSlot itemSlot = GetItemSlot(itemSO);
            SetUpSlotPosition(itemSlot, number);
            itemSlot.SetupItem(itemSO);
        }

        public void AppendItemSlot(BaseItemSO itemSO)
        {
            int number = inventorySO.items.Count - 1;
            ItemSlot itemSlot = GetItemSlot(itemSO);
            SetUpSlotPosition(itemSlot, number);
            itemSlot.SetupItem(itemSO);
        }

        private ItemSlot GetItemSlot(BaseItemSO itemSO)
        {
            ItemSlot itemSlot = factory.GetProduct(itemSlots).GetComponent<ItemSlot>();
            return itemSlot;
        }

        private void SetUpSlotPosition(ItemSlot itemSlot, int number)
        {
            Vector2 position = AllocSlotPosition(number);
            itemSlot.transform.localPosition = position;
        }

        private void RemoveItemSlot(BaseItemSO itemSO, int index)
        {
            Debug.Log($"Begin removing item {itemSO}");
            foreach (Transform child in itemSlots)
            {
                ItemSlot slot = child.GetComponent<ItemSlot>();
                if (slot.itemSOSlot.gameObjectReference == itemSO.gameObjectReference)
                {
                    Debug.Log($"found item, now pooling, item is {slot.itemSOSlot.name}");
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

        private void CheckPosition()
        {

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

