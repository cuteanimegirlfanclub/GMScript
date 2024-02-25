using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    public abstract class BehaviourSO : ScriptableObject
    {
        public abstract void Execute(StateMachineWrapper controller);
    }

    public abstract class StateTriggerBehaviourSO : ScriptableObject
    {
        public abstract void Execute(StateMachineWrapper controller, bool key);
    }
}
