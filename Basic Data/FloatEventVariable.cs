using GMEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMEngine.Value {

    [CreateAssetMenu(fileName = "Float Variable", menuName = "Scriptable Object/Variable/Float Variable Event")]
    public class FloatVariableEvents : BaseVariable
    {
        [SerializeField]
        private float value;

        /// <summary>
        /// the delegated method should have a float value as input.
        /// </summary>
        public UnityEvent<float> onValueChangeEvent;

        public override float Value { get => this.value; set => this.value = value ; }

        public override float ReadValue()
        {
            return value;
        }

        public override void SetValue(float value)
        {
            if (this.value != value)
            {
                OnValueChange(value);
                //Debug.Log("Value Set");
            }
            this.value = value;
        }

        private void OnValueChange(float value)
        {
            onValueChangeEvent?.Invoke(value);
            //Debug.Log("i call event of value " + value);
        }
    }
}
