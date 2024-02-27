using GMEngine.Editor.String.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMEngine.Value
{
    [CustomPropertyDrawer(typeof(BooleanReferenceRO))]
    public class BooleanReferenceRODrawer : PropertyDrawer
    {
        private string propertyName { get; set; }
        private VisualElement root { get; set; }
        private Box Box { get; set; }
        private VisualElement valueContainer { get; set; }
        private VisualElement variableContainer { get; set; }

        private SerializedProperty variableSOProperty { get; set; }
        private SerializedProperty constantValueProperty { get; set; }
        private SerializedProperty useConstantBoolenProperty { get; set; }

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
            variableSOProperty = property.FindPropertyRelative("variable");
            constantValueProperty = property.FindPropertyRelative("constantValue");

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

