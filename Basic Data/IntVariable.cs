using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine {

    [CreateAssetMenu(fileName = "Int Variable", menuName = "Scriptable Object/Variable/Int Variable")]
    public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        public float value;

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }

    [Serializable]
    public class IntReference
    {
        public bool UseConstant = false;
        public float ConstantValue;
        public IntVariable Variable;

        public float Value { get => UseConstant ? ConstantValue : Variable.value; }

        public float DecreaseValue(float value)
        {
            if (UseConstant) { return ConstantValue; }
            return Variable.value -= value;
        }

        public float AddValue(float value)
        {
            if (UseConstant) { return ConstantValue; }
            return Variable.value += value;
        }
    }
}



