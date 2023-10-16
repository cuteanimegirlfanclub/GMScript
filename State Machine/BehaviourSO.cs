using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    public abstract class BehaviourSO : ScriptableObject
    {
        public abstract void Execute(StateMachineController controller);
    }

    public abstract class StateTriggerBehaviourSO : ScriptableObject
    {
        public abstract void Execute(StateMachineController controller, bool key);
    }

    public abstract class InputBehaviourSO : ScriptableObject
    {
        public abstract void BindBehaviour(InputAction.CallbackContext context);
    }
}
