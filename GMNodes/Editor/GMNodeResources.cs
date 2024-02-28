using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMEngine.GMNodes
{
    public static class GMNodeResources
    {
        public static string ResourcesPath => "Assets/GMEngine/Scripts/GMNodes/Resources";

        public static string NodeViewUxmlName => "GMNodeView";
        public static string NodeViewUxmlFilePath = $"{ResourcesPath}/{NodeViewUxmlName}.uxml";

        public static StyleSheet InspectorElementStyle
        {
            get => AssetDatabase.LoadAssetAtPath<StyleSheet>($"{ResourcesPath}/GMInspectorElementStyle.uss");
        }
    }
}

