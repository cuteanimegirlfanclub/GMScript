using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/StateMachine/SatateMachine Wrapper")]
    public class StateMachineWrapper : ScriptableObject
    {
        public StateSO currentState;

        public virtual void UpdateState()
        {
            currentState.UpdateState(this);
        }

        public virtual void PhysicsUpdate()
        {
            currentState.PhysicsUpdate(this);
        }

        public virtual void SetState(StateSO nextState)
        {
            currentState.OnStateExit(this);
            currentState = nextState;
            currentState.OnStateEnter(this);
        }

    }
}

