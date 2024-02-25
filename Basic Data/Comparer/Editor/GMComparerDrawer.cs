using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using GMEngine.UI;
using GMEngine.Editor.UI;
using log4net.Repository.Hierarchy;
using System;

namespace GMEngine.Value
{
    [CustomPropertyDrawer(typeof(ValueComparer))]
    public class GMComparerDrawer : PropertyDrawer
    {
        private Dictionary<int,string> conditionNamePairs = new Dictionary<int,string>();
        private VisualElement root;
        private InspectorElement inspectorElement;

        private ObjectField SOField;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            root = new VisualElement();
            inspectorElement = new InspectorElement(property.serializedObject);
            root.Add(inspectorElement);

            //SOField = new ObjectField();
            //SOField.RegisterValueChangedCallback(OnSOFieldValueChanged);

            //root.Add(SOField);
            return root;
        }

    }
}

