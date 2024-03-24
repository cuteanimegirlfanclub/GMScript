using GMEngine.Value;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine {

    [CreateAssetMenu(fileName = "Int Variable", menuName = "Scriptable Object/Variable/Int Variable")]
    public class IntVariable : ValueVariable<int>
    {
        [SerializeField]
        private int value;

        public override int Value => value;

        public override void SetValue(int value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class IntReferenceRO : IValueReferenceRO
    {
        public bool UseConstant = false;
        public int ConstantValue;
        [SerializeField]
        private IntVariable variable;
        public IntVariable Variable { get => variable; set => variable = value; }

        public int Value { get => UseConstant ? ConstantValue : Variable.Value; }

        public ScriptableObject GetVariableSO()
        {
            return variable;
        }
    }

    [Serializable]
    public class IntReferenceRW
    {
        [SerializeField]
        private IntVariable variable;
        public IntVariable Variable { get => variable; set => variable = value; }
        public int Value { get => Variable.Value; set => Variable.SetValue(value); }
        public void WriteValue(int value)
        {
            Variable.SetValue(value);
        }
        public int ReadValue()
        {
            return Value;
        }

        public IntVariable GetVariableSO()
        {
            return Variable;
        }

    }
}



