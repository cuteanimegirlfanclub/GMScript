using System;
using UnityEngine;
using UnityEngine.Events;

namespace GMEngine.Value
{
    public abstract class ValueVariable<T> : ScriptableObject where T : IComparable
    {
        public abstract void SetValue(T value);

        public abstract T Value { get; }
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Float Variable", menuName = "Scriptable Object/Variable/Float Variable")]
    public class FloatVariable : ValueVariable<float>
    {
        [SerializeField]
        private float value;

        public override float Value => value;

        public float ReadValue()
        {
            return value;
        }

        public override void SetValue(float value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class FloatReferenceRO : ValueReference
    {
        [SerializeField]
        private FloatVariable variable;

        public bool useConstant = false;
        public float constantValue;
        public float Value => useConstant ? constantValue : variable.Value;

        public override ScriptableObject GetVariableSO()
        {
            if( useConstant ) { return null; } else
            {
                return variable;
            }
        }
    }

    [Serializable]
    public class FloatReferenceRW
    {
        [SerializeField]
        private FloatVariable variable;
        public FloatVariable Variable { get 
            {
                if(variable == null)
                {
                    Debug.LogWarning($"{this.GetType()} Lack of Variables!");
                }
                return variable;
            }
            set => 
                variable = value; }
        public float Value { get => Variable.Value;set => Variable.SetValue(value); }

        public void WriteValue(float value)
        {
            Variable.SetValue(value);
        }

        public float ReadValue()
        {
            return Value;
        }
    }

    [Obsolete]
    [Serializable]
    public class FloatReferenceSimpleRW
    {
        public bool UseConstant = false;
        public float ConstantValue;
        [SerializeField]
        private FloatVariable Variable;

        #region Constructor
        public FloatReferenceSimpleRW(bool useConstant, float constantValue, FloatVariable variable)
        {
            UseConstant = useConstant;
            ConstantValue = constantValue;
            Variable = variable;
        }

        public FloatReferenceSimpleRW(string variableName, float variableValue)
        {
            UseConstant = false;
            this.Variable = ScriptableObject.CreateInstance<FloatVariable>();
            Variable.name = variableName;
            Variable.SetValue(variableValue);
        }
        public FloatReferenceSimpleRW(string variableName, FloatVariable variable)
        {
            UseConstant = false;
            Variable = variable;
            Variable.name = variableName;
        }
        public FloatReferenceSimpleRW(bool useConstant, float constantValue, string variableName, float variableValue)
        {
            UseConstant = useConstant;
            ConstantValue = constantValue;
            Variable = ScriptableObject.CreateInstance<FloatVariable>();
            Variable.name = variableName;
            Variable.SetValue(variableValue);
        }
        #endregion

        public float Value => UseConstant ? ConstantValue : Variable.Value;

        public void DecreaseValue(float value)
        {
            Variable.SetValue(-value);
        }

        public void AddValue(float value)
        {
            Variable.SetValue(value);
        }
    }

}

