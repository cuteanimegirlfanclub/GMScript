using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.Value;
using UnityEngine.InputSystem;

namespace GMEngine{
    //Deprecated
    [CreateAssetMenu(menuName = "Scriptable Object/Behaviours/InputActionBinder")]
    public class InputActionBinder : StateTriggerBehaviourSO
    {
        public InputAction inputAction;

        public InputBehaviourSO[] inputBehavioursToBind;

        public override void Execute(StateMachineController controller, bool key)
        {
            if (key)
            {
                Bind(inputBehavioursToBind);
            }
            else
            {
                UnBind(inputBehavioursToBind);
            }
        }

        private void Bind(InputBehaviourSO[] behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                inputAction.performed += behaviour.BindBehaviour;
            }
            inputAction.Enable();
        }

        private void UnBind(InputBehaviourSO[] behaviours)
        {
            inputAction.Disable(); 
            foreach (var behaviour in behaviours)
            {
                inputAction.performed -= behaviour.BindBehaviour;
            }
        }
    }
}

