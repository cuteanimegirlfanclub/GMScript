using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GMEngine.Value
{
    [Serializable]
    [CreateAssetMenu(fileName = "Boolean Variable", menuName = "Scriptable Object/Variable/Boolean Variable")]
    public class BooleanVariable : ValueVariable<bool>
    {
        [SerializeField]
        private bool value;
        public override bool Value => value;

        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class BooleanReferenceRO : IValueReferenceRO
    {
        [SerializeField]
        private BooleanVariable variable;

        public bool useConstant = false;
        public bool constantValue;
        public bool Value => useConstant ? constantValue : variable.Value;

        public ScriptableObject GetVariableSO()
        {
            if (useConstant) { return null; }
            else
            {
                return variable;
            }
        }
    }

    [Serializable]
    public class BooleanReferenceRW : IValueReferenceRW
    {
        [SerializeField]
        private BooleanVariable variable;
        public BooleanVariable Variable { get => variable; set => variable = value; }
        public bool Value { get => Variable.Value; set => Variable.SetValue(value); }

        public void WriteValue(bool value)
        {
            Variable.SetValue(value);
        }

        public bool ReadValue()
        {
            return Value;
        }

        public ScriptableObject GetVariableSO()
        {
            return variable;
        }
    }
}

