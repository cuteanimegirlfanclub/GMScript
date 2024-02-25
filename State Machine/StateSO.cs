using UnityEngine;
using System;
using GMEngine.Prototype;

namespace GMEngine
{
    //need to be development
    [CreateAssetMenu(fileName = "State", menuName = "Scriptable Object/StateMachine/State")]
    public class StateSO : ScriptableObject, IPrototype<StateSO>
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
        public void UpdateState(StateMachineWrapper controller)
        {
            CheckTransitions(controller);
            UpdateLoopBehaviour(controller);
        }

        public void PhysicsUpdate(StateMachineWrapper controller)
        {
            PhysicsLoopBehaviour(controller);
        }
        
        public void UpdateLoopBehaviour(StateMachineWrapper controller)
        {
            foreach (var behaviour in updateBehaviours)
            {
                ExecuteBehaviours(controller, behaviour);
            }
        }

        public void PhysicsLoopBehaviour(StateMachineWrapper controller)
        {
            foreach (var behaviour in physicsBehaviours)
            {
                ExecuteBehaviours(controller, behaviour);
            }
        }

        public void OnStateEnter(StateMachineWrapper controller)
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
        public void OnStateExit(StateMachineWrapper controller)
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

        private void ExecuteBehaviours(StateMachineWrapper controller, BehaviourSO behaviour)
        {
            behaviour.Execute(controller);
        }

        private void CheckTransitions(StateMachineWrapper controller)
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

        public StateSO DeepCopy()
        {
            throw new NotImplementedException();
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
