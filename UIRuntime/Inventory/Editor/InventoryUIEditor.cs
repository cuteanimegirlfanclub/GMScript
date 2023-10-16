using GMEngine;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Linq;

namespace GMEngine.Editor.UI
{
    [CustomEditor(typeof(InventoryUI))]
    public class InventoryUIEditor : UnityEditor.Editor
    {
        private int maxItem;
        private string prefabPath;
        VisualElement root { get; set; }
        Button displayButton { get; set; }

        private void Awake()
        {
            InventoryUI ui = target as InventoryUI;
            prefabPath = AssetDatabase.GetAssetPath(ui.itemSlotPrefab);
        }

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement(); 
            root.AddDefaultInspector(this);
            displayButton = root.AddButton("Display");
            displayButton.clickable.activators.Clear();
            displayButton.RegisterCallback<MouseDownEvent>(CreateEditorDisplay);
            displayButton.RegisterCallback<MouseUpEvent>(ClearEditorDisplay);

            root.AddButton("Clear").clickable.clicked += ClearEditorDisplayInternal;
            //root.AddButton("Apply").clickable.clicked += ApplySettingChange;

            root.Add("Calculate", CalculateInventorySetting);
            return root;
        }

        private void CreateEditorDisplay(MouseDownEvent evt)
        {
            InventoryUI ui = target as InventoryUI;
            ClearEditorDisplayInternal();

            for (int i = 0; i < maxItem; i++)
            {
                AppendEditorItemSlot(ui, i);
            }
        }

        private void ClearEditorDisplay(MouseUpEvent evt)
        {
            ClearEditorDisplayInternal();
        }

        private void ClearEditorDisplayInternal()
        {
            InventoryUI ui = target as InventoryUI;
            Transform itemSlots = ui.itemSlots;

            if (itemSlots != null)
            {
                var tempList = itemSlots.transform.Cast<Transform>().ToList();
                foreach (Transform child in tempList)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private void AppendEditorItemSlot(InventoryUI inventoryUI, int number)
        {
            GameObject itemSlot = PrefabUtility.InstantiatePrefab(inventoryUI.itemSlotPrefab) as GameObject;
            itemSlot.transform.SetParent(inventoryUI.itemSlots);
            itemSlot.transform.localPosition = inventoryUI.AllocSlotPosition(number);
            itemSlot.transform.localScale = Vector3.one;
        }

        private void CalculateRowInterval(InventoryUI inventoryUI)
        {
            float totalHeight = inventoryUI.GetComponent<RectTransform>().rect.height - inventoryUI.heightOffset;
            float slotHeight = inventoryUI.itemSlotPrefab.GetComponent<RectTransform>().rect.height;

            int maxRow = Mathf.FloorToInt(totalHeight / slotHeight);

            float RowL = slotHeight * (float)maxRow;

            float remainingHeight = totalHeight - RowL;

            inventoryUI.rowInterval = (remainingHeight / maxRow) + slotHeight;
            inventoryUI.maxRow = maxRow;
            //Debug.Log($"maxColumn: {maxRow}, remainingWidth: {remainingHeight}");
        }

        private void CalculateColumnInterval(InventoryUI inventoryUI)
        {
            float totalWidth = inventoryUI.GetComponent<RectTransform>().rect.width - inventoryUI.widthOffset;
            float slotWidth = inventoryUI.itemSlotPrefab.GetComponent<RectTransform>().rect.width;

            int maxColumn = Mathf.FloorToInt(totalWidth / slotWidth);

            float ColumnL = slotWidth * (float)maxColumn;

            float remainingWidth = totalWidth - ColumnL;

            inventoryUI.columnInterval = (remainingWidth / maxColumn) + slotWidth;
            inventoryUI.maxColumn = maxColumn;
            //Debug.Log($"maxColumn: {maxColumn}, remainingWidth: {remainingWidth}");

        }

        private void CalculateMaxItem(InventoryUI inventoryUI)
        {
            maxItem = inventoryUI.maxColumn * inventoryUI.maxRow;
        }

        private void OnEnable()
        {
            CalculateInventorySetting();
        }

        private void CalculateInventorySetting()
        {
            InventoryUI inventoryUI = (InventoryUI)target;
            CalculateColumnInterval(inventoryUI);
            CalculateRowInterval(inventoryUI);
            CalculateMaxItem(inventoryUI);
        }

        private void ApplySettingChange()
        {
            InventoryUI ui = (InventoryUI)target;
            PrefabUtility.ApplyObjectOverride(ui.gameObject, prefabPath, InteractionMode.UserAction);
        }
    }


}
