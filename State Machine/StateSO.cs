using UnityEngine;
using System;

namespace GMEngine
{
    //need to be development
    [CreateAssetMenu(fileName = "State", menuName = "Scriptable Object/StateMachine/State")]
    public class StateSO : ScriptableObject
    {
        [Header("Behaviours")]
        public BehaviourSO[] updateBehaviours;
        public BehaviourSO[] physicsBehaviours;

        public BehaviourSO[] enterStateBehaviours;
        public BehaviourSO[] exitStateBehaviours;

        public StateTriggerBehaviourSO[] triggerBehaviours;

        [Header("Transition Tree")]
        public Transition[] transitions;

        /// <summary>
        /// per frame state check & persitent behaviour execute
        /// </summary>
        /// <param name="controller"></param>
        public void UpdateState(StateMachineController controller)
        {
            CheckTransitions(controller);
            UpdateLoopBehaviour(controller);
        }

        public void PhysicsUpdate(StateMachineController controller)
        {
            PhysicsLoopBehaviour(controller);
        }
        
        public void UpdateLoopBehaviour(StateMachineController controller)
        {
            foreach (var behaviour in updateBehaviours)
            {
                ExecuteBehaviours(controller, behaviour);
            }
        }
        public void PhysicsLoopBehaviour(StateMachineController controller)
        {
            foreach (var behaviour in physicsBehaviours)
            {
                ExecuteBehaviours(controller, behaviour);
            }
        }

        public void OnStateEnter(StateMachineController controller)
        {
            foreach (var behaviour in enterStateBehaviours)
            {
                ExecuteBehaviours(controller, behaviour);
            }

            foreach (var behaviour in triggerBehaviours)
            {
                behaviour.Execute(controller, true);
            }
        }
        public void OnStateExit(StateMachineController controller)
        { 
            foreach (var behaviour in exitStateBehaviours)
            {
                ExecuteBehaviours(controller, behaviour);
            }

            foreach (var behaviour in triggerBehaviours)
            {
                behaviour.Execute(controller, false);
            }
        }

        private void ExecuteBehaviours(StateMachineController controller, BehaviourSO behaviour)
        {
            behaviour.Execute(controller);
        }

        private void CheckTransitions(StateMachineController controller)
        {
            foreach (Transition transition in transitions)
            {
                bool shouldTransition = transition.condition.CheckCondition(controller);
                if (shouldTransition)
                {
                    controller.SetState(transition.toState);
                }
            }
        }
    }

    //it could be optimized into StateTree
    [Serializable]
    public class Transition
    {
        /// <summary>
        /// Condition When To Change State
        /// </summary>
        public ConditionSO condition;
        /// <summary>
        /// The State You Want to Transit Into
        /// </summary>
        public StateSO toState;
    }

}
