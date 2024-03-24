using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.SceneManagement;
using GMEngine.StringExtension;


namespace GMEngine.Game
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        private readonly string configurePath = "Assets/GMEngine/GMResources/Level/Configure";
        private readonly string groupName = "Configure";
        private AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Get Configure"))
            {
                GetConfigure((LevelManager)target);
            }
        }
        private void GetConfigure(LevelManager manager)
        {
            string scneName = SceneManager.GetActiveScene().name;
            LevelConfigure configure = AssetDatabase.LoadAssetAtPath<LevelConfigure>($"{configurePath}/{scneName.SceneToConfigure()}");
            if (configure == null)
            {
                Debug.LogWarning($"Cannot Find Configure {scneName.SceneToConfigure()}, Creating New...");

                //Create new configure asset to disk
                LevelConfigure newCon = ScriptableObject.CreateInstance<LevelConfigure>();
                string path = $"{configurePath}/{scneName}Configure.asset";
                AssetDatabase.CreateAsset(newCon, path);
                AssetDatabase.SaveAssets();
                manager.configure = newCon;

                //mark the asset as addressable
                AddressableAssetGroup group = settings.FindGroup(groupName);
                string guid = AssetDatabase.AssetPathToGUID(path);
                var entry = settings.CreateOrMoveEntry(guid, group);
                entry.address = $"{scneName}Configure";

            }
            else
            {
                Debug.LogWarning($"Get Existed Configure {scneName.SceneToConfigure()}");
                manager.configure = configure;
            }
        }
    }
}

