using UnityEngine;
using UnityEditor;
using GMEngine;

public class SaveDataEditorWindow : EditorWindow
{
    private SaveData saveData;


    [MenuItem("GMEngine/Save Data Debuger")]
    public static void ShowWindow()
    {
        GetWindow<SaveDataEditorWindow>("Save Data Debuger");
    }

    private void OnGUI()
    {
        GUILayout.Label("Save Data Editor", EditorStyles.boldLabel);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Data File:", GUILayout.Width(100));

        if (GUILayout.Button("Open"))
        {
            string path = EditorUtility.OpenFilePanel("Open Save Data", Application.persistentDataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                saveData = LoadSaveData(path);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (saveData != null)
        {
            GUILayout.Label("Save Data Details", EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Scene Build Index: " + saveData.sceneBuildIndex);
            EditorGUILayout.Vector3Field("Player Position", saveData.playerPosition);
            //EditorGUILayout.QuaternionField("Player Rotation", saveData.playerRotation);
            EditorGUILayout.FloatField("Player Health", saveData.playerHealth);
            EditorGUILayout.FloatField("Player Mental", saveData.playerMental);

            GUILayout.Space(15);

            GUILayout.Label("Ground Item Data", EditorStyles.boldLabel);
            if(saveData.groundItemDatas.Count == 0)
            {
                EditorGUILayout.LabelField("No Ground Item Data Found");
            }

            foreach (var itemData in saveData.groundItemDatas)
            {
                EditorGUILayout.LabelField("Name: " + itemData.itemName);
                EditorGUILayout.ObjectField("Base Item SO", itemData.baseItemSO, typeof(BaseItemSO), false);
                EditorGUILayout.Vector3Field("Position", itemData.position);
                //EditorGUILayout.QuaternionField("Rotation", itemData.rotation);

                if (itemData.baseItemSO is StackableItemSO stackableItem)
                {
                    EditorGUILayout.FloatField("Number", stackableItem.number);
                }
            }

            GUILayout.Space(15);

            GUILayout.Label("Inventory Item Data", EditorStyles.boldLabel);
            if (saveData.inventoryItemDatas.Count == 0)
            {
                EditorGUILayout.LabelField("No Ground Item Data Found", EditorStyles.boldLabel);
            }
            foreach (var itemData in saveData.inventoryItemDatas)
            {
                EditorGUILayout.LabelField("Name: " + itemData.itemName);
                EditorGUILayout.ObjectField("Base Item SO", itemData.baseItemSO, typeof(BaseItemSO), false);
            }

            if (saveData.inventoryItemDatas.Count != 0)
            {
                EditorGUILayout.IntField("Hand Item Number", saveData.handItemNum);
            }
        }
    }

    private SaveData LoadSaveData(string path)
    {
        if (System.IO.File.Exists(path))
        {
            using (var reader = new System.IO.BinaryReader(System.IO.File.Open(path, System.IO.FileMode.Open)))
            using (var saveDataReader = new SaveDataReader(reader))
            {
                SaveData data = CreateInstance<SaveData>();
                data.Load(saveDataReader);
                return data;
            }
        }
        else
        {
            Debug.LogError("Save Data file not found: " + path);
            return null;
        }
    }
}