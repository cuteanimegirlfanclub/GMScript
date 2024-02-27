using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMEngine.GMNodes
{
    public static class GMNodeResources
    {
        public static string NodeViewUxmlFilePath = "Assets/GMEngine/Scripts/GMNodeGraph/Resources/GMNodeView.uxml";
        public static StyleSheet InspectorElementStyle
        {
            get => AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GMEngine/Scripts/GMNodeGraph/Resources/GMInspectorElementStyle.uss");
        }
    }
}

