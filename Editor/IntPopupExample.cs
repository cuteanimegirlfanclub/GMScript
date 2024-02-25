// Simple Editor Script that lets you rescale the current selected GameObject.
using UnityEditor;
using UnityEngine;

public class IntPopupExample : EditorWindow
{
    int selectedSize = 1;
    string[] names = new string[] { "Normal", "Double", "Quadruple" };
    int[] sizes = { 1, 2, 4 };

    [MenuItem("Examples/Int Popup usage")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(IntPopupExample));
        window.Show();
    }

    void OnGUI()
    {
        selectedSize = EditorGUILayout.IntPopup("Resize Scale: ", selectedSize, names, sizes);
        if (GUILayout.Button("Scale"))
            ReScale();
    }

    void ReScale()
    {
        if (Selection.activeTransform)
            Selection.activeTransform.localScale =
                new Vector3(selectedSize, selectedSize, selectedSize);
        else
            Debug.LogError("No Object selected, please select an object to scale.");
    }
}
