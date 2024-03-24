using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.NodeGraph;
using UnityEditor;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using System;
using Unity.Properties;
using System.Text;

namespace GMEngine.GMNodes
{
    public static class GMNodeGraphUtility
    {
        private readonly static string[] excludes = { "PropertyField:m_Script", "position", "children" };
        private const BindingFlags AllBindingFlags = (BindingFlags)(-1);

        public static string NameStripping(string text)
        {
            return text.Replace("GM", "").Replace("Node", " Node");
        }

        public static string[] GetPropertyExcludes()
        {
            return excludes;
        }

        public static bool IsExcludedProperty(this string propertyName)
        {
            Debug.Log($"Checking if {propertyName} exclued..");
            return excludes.Contains(propertyName);
        }

        public static string GetUniqueNodeName(this string originalName, List<GMNode>nodes)
        {
            string newName = originalName;
            int suffix = 1;
            while (nodes.Any(n => n.name == newName))
            {
                newName = $"{originalName}_{suffix}";
                suffix++;
            }
            return newName;
        }

        public static T FindAttribute<T>(this Type type) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(type, typeof(T));
            if (attribute != null)
            {
                return attribute;
            }

            Type parentType = type.BaseType;
            while (parentType != null)
            {
                attribute = (T)Attribute.GetCustomAttribute(parentType, typeof(T));
                if (attribute != null)
                {
                    return attribute;
                }
                parentType = parentType.BaseType;
            }

            return null;
        }

        public static void PrintObjectDump<T>(T value)
        {
            GMPropertyVisitor s_Visitor = new GMPropertyVisitor();
            s_Visitor.Reset();

            PropertyContainer.Accept(s_Visitor, ref value);
            Debug.Log(s_Visitor.GetDump());
        }

        public static List<string> GetPropertiesWithAttribute(this SerializedObject serializedObject, Type attributeType)
        {
            var iterator = serializedObject.GetIterator();
            List<string> propertyNames = new List<string>();
            while (iterator.NextVisible(true))
            {
                if (iterator.name == "m_Script") continue;
                var attributes = iterator.GetAttributes<HideInNodeBodyAttribute>(true);
                if (attributes != null)
                {
                    if (attributes.Length > 0)
                    {
                        propertyNames.Add($"PropertyField:{iterator.name}");
                    }
                }
            }
            return propertyNames;
        }

        public static TAttribute[] GetAttributes<TAttribute>(this SerializedProperty serializedProperty, bool inherit)
    where TAttribute : PropertyAttribute
        {
            if (serializedProperty == null)
            {
                throw new ArgumentNullException(nameof(serializedProperty));
            }

            Type targetObjectType = serializedProperty.serializedObject.targetObject.GetType();
            Debug.Log($"Getting {targetObjectType}'s property");

            if (targetObjectType == null)
            {
                throw new ArgumentException($"Could not find the {nameof(targetObjectType)} of {nameof(serializedProperty)}");
            }

            foreach (string pathSegment in serializedProperty.propertyPath.Split('.'))
            {
                var fieldInfo = GetFieldInfo(targetObjectType, pathSegment);
                if (fieldInfo != null)
                {
                    Debug.Log($"Getting {targetObjectType}'s {fieldInfo}");
                    return (TAttribute[])fieldInfo.GetCustomAttributes<TAttribute>(inherit);
                }

                var propertyInfo = GetPropertyInfo(targetObjectType, pathSegment);
                if (propertyInfo != null)
                {
                    Debug.Log($"Getting {targetObjectType}'s {propertyInfo}");
                    return (TAttribute[])propertyInfo.GetCustomAttributes<TAttribute>(inherit);
                }
            }
            Debug.LogWarning($"Could not find the field or property of {serializedProperty.name}");
            return null;
        }

        public static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            FieldInfo fieldInfo = null;
            while (type != null && fieldInfo == null)
            {
                fieldInfo = type.GetField(fieldName, AllBindingFlags);
                type = type.BaseType;
            }
            return fieldInfo;
        }

        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo propertyInfo = null;
            while (type != null && propertyInfo == null)
            {
                propertyInfo = type.GetProperty(propertyName, AllBindingFlags);
                type = type.BaseType;
            }
            return propertyInfo;
        }
        public class GMPropertyVisitor : PropertyVisitor
        {
            private const int k_InitialIndent = 0;
            private readonly StringBuilder m_Builder = new StringBuilder();

            private int m_IndentLevel = k_InitialIndent;

            private string Indent => new(' ', m_IndentLevel * 2);

            public void Reset()
            {
                m_Builder.Clear();
                m_IndentLevel = k_InitialIndent;
            }

            public string GetDump()
            {
                return m_Builder.ToString();
            }

            public List<string> GetPropertyList()
            {
                return m_Builder.ToString().Split("").ToList();
            }

            protected override void VisitProperty<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
            {
                m_Builder.AppendLine($"- {property.Name}");
            }
        }
    }

}
