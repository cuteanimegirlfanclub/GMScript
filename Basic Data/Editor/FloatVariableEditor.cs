using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GMEngine.Value;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using GMEngine.String.Extension;
using System;

namespace GMEngine.UtilEditor
{
    [CustomPropertyDrawer(typeof(FloatReferenceRO))]
    public class FloatReferenceRODrawer : PropertyDrawer
    {
        private string propertyName { get; set; }
        private VisualElement root { get; set; }
        private Box Box { get; set; }
        private VisualElement valueContainer { get; set; }
        private VisualElement variableContainer { get; set; }

        private SerializedProperty variableSOProperty { get; set; }
        private SerializedProperty constantValueProperty { get; set; }
        private SerializedProperty useConstantBoolenProperty { get; set; }
        private SerializedProperty variableValueProperty { get; set; }

        private PropertyField variableSOField { get; set; }
        private PropertyField constantValueField { get; set; }

        private Toggle useConstantToggle { get; set; }
        private Label propertyNameLabel { get; set; }


        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            FindProperties(property);
            InitializeDrawer();
            Composite();
            ApplyStyle();
            return root;
        }
        private void FindProperties(SerializedProperty property)
        {
            variableSOProperty = property.FindPropertyRelative("Variable");
            constantValueProperty = property.FindPropertyRelative("constantValue");
            variableValueProperty = property.FindPropertyRelative("Variable/value");

            useConstantBoolenProperty = property.FindPropertyRelative("useConstant");
            propertyName = property.name;
        }
        private void InitializeDrawer()
        {
            root = new VisualElement();

            Box = new Box();

            valueContainer = new VisualElement();
            variableContainer = new VisualElement();

            propertyNameLabel = new Label(propertyName.ConvertToTitleCase());

            useConstantToggle = new Toggle();
            useConstantToggle.text = "Use Constant Value";
            useConstantToggle.BindProperty(useConstantBoolenProperty);
            useConstantToggle.RegisterValueChangedCallback(evt => UseConstantValue(evt.newValue));

            variableSOField = new PropertyField(variableSOProperty); 
            constantValueField = new PropertyField(constantValueProperty);
        }

        private void Composite()
        {
            root.Add(Box);

            valueContainer.Add(constantValueField);

            variableContainer.Add(variableSOField); 

            if (variableValueProperty != null)
            {
                var floatField = new FloatField();
                floatField.value = variableValueProperty.floatValue;
                floatField.RegisterValueChangedCallback((evt) =>
                {
                    variableValueProperty.floatValue = evt.newValue;
                });
                variableContainer.Add(floatField);
            }

            Box.Add(propertyNameLabel);
            Box.Add(useConstantToggle);
            Box.Add(valueContainer);
            Box.Add(variableContainer);
        }

        private void ApplyStyle()
        {
            propertyNameLabel.style.borderTopWidth = 3;
            propertyNameLabel.style.borderBottomWidth = 3;
            propertyNameLabel.style.fontSize = 14;
            Box.style.borderBottomWidth = 5;
        }

        private void UseConstantValue(bool newValue)
        {
            valueContainer.style.display = newValue ? DisplayStyle.Flex : DisplayStyle.None;
            variableContainer.style.display = newValue ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}
