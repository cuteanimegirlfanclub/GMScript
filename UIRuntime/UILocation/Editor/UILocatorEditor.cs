using UnityEngine.UIElements;
using UnityEditor;
using GMEngine.UI;
using UnityEngine;

namespace GMEngine.Editor.UI
{
    [CustomEditor(typeof(UILocator))]
    public class UILocatorEditor : UnityEditor.Editor
    {
        VisualElement root { get; set; }
        Button displayButton { get; set; }

        public override VisualElement CreateInspectorGUI()
        {
            root = new VisualElement();
            root.AddDefaultInspector(this);

            displayButton = root.AddButton("Set And Display");
            displayButton.clickable.activators.Clear();
            displayButton.RegisterCallback<MouseDownEvent>(DisplayUILocation);
            displayButton.RegisterCallback<MouseUpEvent>(SetAndClear);

            return root;
        }

        private void DisplayUILocation(MouseDownEvent evt)
        {
            UILocator locator = target as UILocator;
            if (locator.UISceneGO) { Debug.Log("Preview Object Already Existed"); return; }
            locator.UISceneGO = Instantiate(locator.UIPrefab, locator.transform);
            Debug.Log($"Displaying {locator.UIPrefab.name}");

        }

        private void SetAndClear(MouseUpEvent evt)
        {
            UILocator locator = target as UILocator;
            if (locator.UISceneGO)
            {
                Vector2 loactedPositionSP = Camera.main.WorldToScreenPoint(locator.locatingGO.transform.position);
                Vector2 targetPositionSP = Camera.main.WorldToScreenPoint(locator.transform.position);
                locator.offset = targetPositionSP - loactedPositionSP;

                Debug.Log($"UI Position Located, Offset is {locator.offset}");
                DestroyImmediate(locator.UISceneGO);
            }
        }
    }

}

