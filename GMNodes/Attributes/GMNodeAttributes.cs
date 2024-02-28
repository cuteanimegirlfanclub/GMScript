using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

namespace GMEngine.GMNodes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HideInNodeBodyAttribute : PropertyAttribute { 

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomNodeEditorAttribute : Attribute
    {
        Type inspecptingType;
        public CustomNodeEditorAttribute(Type type)
        {
            inspecptingType = type;
        }

        public Type GetInspectingType()
        {
            return inspecptingType;
        }
    }
}

