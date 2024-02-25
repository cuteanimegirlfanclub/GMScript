using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Reflection;

namespace GMEngine.Editor.UI
{
    public static class UIElementExtension
    {
        #region Add

        /// <summary>
        /// Quickly add a button with cliked(OnMouseClicked) event.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="topic"></param>
        /// <param name="clickEvt"></param>
        /// <returns></returns>
        public static VisualElement Add(this VisualElement element, string topic, Action clickEvt)
        {
            ElementCheck(element);

            Button button = new Button();
            button.text = topic;
            button.clickable.clicked += clickEvt;
            element.Add(button);
            return element;
        }

        /// <summary>
        /// Quickly add a button and return the instance of the button.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        public static Button AddButton(this VisualElement element, string topic)
        {
            ElementCheck(element);

            Button button = new Button();
            button.text = topic;
            element.Add(button);
            return button;
        }

        public static VisualElement Add(this VisualElement element, string propertyName)
        {
            //SerializedProperty property = UnityEditor.Editor.serializedObject.FindProperty(propertyName);
            return element;
        }

        private static VisualElement ElementCheck(VisualElement element)
        {
            if (element == null)
            {
                Debug.LogError("VisualElement is null.");
            }
            return element;
        }
        #endregion


        public static void AddDefaultInspector(this VisualElement root, UnityEditor.Editor editor)
        {
            InspectorElement.FillDefaultInspector(root, editor.serializedObject, editor);
        }
    }
}

