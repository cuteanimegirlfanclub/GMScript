using System;
using UnityEngine;
using UnityEngine.Events;

namespace GMEngine.Value
{
    public abstract class BaseVariable : ScriptableObject
    {
        public abstract void SetValue(float value);
        public abstract float ReadValue();

        public abstract float Value {get; set;}
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Float Variable", menuName = "Scriptable Object/Variable/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField]
        private float value;

        public float Value { get => this.value; set => this.value = value; }

        public float ReadValue()
        {
            return value;
        }

        public void SetValue(float value)
        {
            this.value = value;
        }
    }

    public interface IValueReference
    {
        /// <summary>
        /// extremely unsafe method, try not to use it!!!
        /// </summary>
        public BaseVariable GetVariableSO();
    }

    public interface IReadWriteValueReference : IValueReference
    {
        public void SetValue(float value);
        public float ReadValue();
    }

    public interface IReadOnlyValueReference : IValueReference
    {
        public float ReadValue();
    }

    [Serializable]
    public class FloatReferenceRO
    {
        [SerializeField]
        private FloatVariable Variable;

        public float constantValue;
        public bool useConstant = false;
        public float Value => useConstant ? constantValue : Variable.Value;
        public float ReadValue()
        {
            return Value;
        }

        public FloatVariable GetVariableSO()
        {
            return Variable;
        }
    }

    [Serializable]
    public class FloatReferenceRW
    {
        [SerializeField]
        public FloatVariable Variable;
        public float Value { get => Variable.Value; set => Variable.Value = value; }
        public void SetValue(float value)
        {
            Variable.SetValue(value);
        }
        public float ReadValue()
        {
            return Value;
        }

        public FloatVariable GetVariableSO()
        {
            return Variable;
        }
    }

    //[Serializable]
    //public class FloatReferenceRWEvent
    //{
    //    [SerializeField]
    //    public FloatVariable Variable;
    //    public float Value => Variable.Value;

    //    public UnityEvent onValueChangeEvent;
    //    public void SetValue(float value)
    //    {
    //        Variable.Value = value;
    //        onValueChangeEvent?.Invoke();
    //    }

    //}

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

        public float Value => UseConstant ? ConstantValue : Variable.ReadValue();

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

