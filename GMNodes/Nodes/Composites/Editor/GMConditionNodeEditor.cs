using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace GMEngine.GMNodes
{
    [CustomNodeEditor(typeof(GMConditionNode))]
    public class GMConditionNodeEditor : GMNodeEditor
    {
        [HideInInspector]
        public List<string> childrenName = new List<string>();
        [HideInInspector]
        public int[] childrenIndex;

        private VisualElement comparerElement;

        public override void Init()
        {
            GMConditionNode node = (GMConditionNode)target;
            if(node.Comparer == null)
            {
                return;
            }

            node.VerifyCondition();

            childrenName.Clear();
            foreach (GMNode child in node.GetChildrenList())
            {
                childrenName.Add(child.name);
            }
            childrenIndex = new int[node.GetChildrenList().Count];
            for (int i = 0; i < node.GetChildrenList().Count; i++)
            {
                childrenIndex[i] = i;
            }

            comparerElement.Clear();
            if (node.Comparer == null) return;

            comparerElement.Add(new Label("Comparer Editor"));
            InspectorElement comparerInspector = new InspectorElement(node.Comparer);
            comparerInspector.Q("PropertyField:m_Script").RemoveFromHierarchy();
            comparerElement.Add(comparerInspector);
        }

        protected override InspectorElement CreateGUI(InspectorElement element)
        {
            GMConditionNode node = (GMConditionNode)target;

            ObjectField comparerField = BodyElement.Q<ObjectField>();
            comparerField.RegisterValueChangedCallback(OnObjectFieldChange);
            comparerElement = new VisualElement();
            comparerElement.name = "comparer_element";
            element.Add(comparerElement);

            element.Add(new IMGUIContainer(() =>
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                if (node.Comparer != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Conditions");
                    GUILayout.Label("Nodes");
                    GUILayout.EndHorizontal();
                    for (int i = 0; i < node.Comparer.ConditionCount; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(node.Comparer.GetConditionName(i));
                        GUILayout.Space(EditorGUIUtility.fieldWidth);
                        node.conditions[i] = EditorGUILayout.IntPopup(node.conditions[i], childrenName.ToArray(), childrenIndex);
                        GUILayout.EndHorizontal();
                    }
                }
                else
                {
                    GUILayout.Label("Please Assgin Comparer");
                }
            }
            ));
            return element;
        }

        private void OnObjectFieldChange(ChangeEvent<UnityEngine.Object> evt)
        {
            if(evt.newValue == null)
            {
                return;
            }

            Init();
        }

        public override void OnOutputChanged(GMNode node)
        {
            Init();
        }

    }

}
