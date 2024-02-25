using GMEngine.GMXNode.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GMEngine.GMXNode
{
    [CustomNodeEditor(typeof(ConditionGMXNode))]
    public class ConditionGMXNodeEditor : GMXNodeEditor
    {
        public override void OnBodyGUI()
        {
            ConditionGMXNode node = (ConditionGMXNode)target;
            base.OnBodyGUI();
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Conditions");
            GUILayout.Label("Nodes");
            GUILayout.EndHorizontal();

            if(node.Comparer != null)
            {
                for (int i = 0; i < node.Comparer.ConditionCount; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(node.Comparer.GetConditionName(i));
                    node.conditions[i] = EditorGUILayout.IntPopup(node.conditions[i], node.childrenName.ToArray(), node.childrenIndex);

                    GUILayout.EndHorizontal();
                }
            }

        }
    }

}
