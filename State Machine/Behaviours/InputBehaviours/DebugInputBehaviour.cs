using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Behaviours/DebugInputBehaviour")]
    public class DebugInputBehaviour : InputBehaviourSO
    {
        public override void BindBehaviour(InputAction.CallbackContext context)
        {

        }

        private Vector2 GetInputData(InputAction.CallbackContext context)
        {
            return context.ReadValue<Vector2>();
        }
    }

}
