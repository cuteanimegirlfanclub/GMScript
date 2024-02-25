using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Value
{
    [CreateAssetMenu(fileName = "String Variable", menuName = "Scriptable Object/Variable/String Variable")]
    public class StringVariable : ScriptableObject
    {
        [SerializeField]
        private string value;

        public string Value { get => this.value; set => this.value = value; }

        public string ReadValue()
        {
            return value;
        }

        public void SetValue(string value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class StringReferenceRO
    {
        [SerializeField]
        private StringVariable Variable;

        public string constantValue;
        public bool useConstant = false;
        public string Value => useConstant ? constantValue : Variable.Value;
        public string ReadValue()
        {
            return Value;
        }

        public StringVariable GetVariableSO()
        {
            return Variable;
        }
    }

    [Serializable]
    public class StringReferenceRW
    {
        [SerializeField]
        public StringVariable Variable;
        public string Value { get => Variable.Value; set => Variable.Value = value; }
        public void SetValue(string value)
        {
            Variable.SetValue(value);
        }
        public string ReadValue()
        {
            return Value;
        }

        public StringVariable GetVariableSO()
        {
            return Variable;
        }
    }
}


