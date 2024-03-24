using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GMEngine.Game
{
    [CustomEditor(typeof(ItemSpawnerMono))]
    public class ItemSpawnerMonoEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ItemSpawnerMono itemSpawner = (ItemSpawnerMono)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Bake"))
            {
                itemSpawner.Bake();
                EditorUtility.SetDirty(itemSpawner.configure);
            }
        }
    }

}

