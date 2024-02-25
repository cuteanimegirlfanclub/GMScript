using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GMEngine.Editor
{
    public static class WorkFlow
    {
        
    }

    public class WorkFlowWindow : EditorWindow
    {
        public WorkFlowAsset paths;

        [MenuItem("GMEngine/WorkFlow")]
        public static void ShowWindow()
        {
            GetWindow<WorkFlowWindow>("Work Flow Global");
        }

        private void OnGUI()
        {

        }
    }

    public class WorkFlowAsset : ScriptableObject
    {
        [SerializeField]
        private List<string> assetPaths;
    }
}
