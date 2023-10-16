using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Behaviours/Human Move Behaviour")]
    public class HumanMoveBehaviour : InputBehaviourSO
    {
        public FloatReferenceRO speed;

        public AnimatorSO animatorSO;

        public override void BindBehaviour(InputAction.CallbackContext context)
        {
            animatorSO.animator.SetFloat("Vertical Speed", GetInputData(context).magnitude * speed.Value, 0.1f, Time.deltaTime);
        }

        private Vector2 GetInputData(InputAction.CallbackContext context)
        {
            return context.ReadValue<Vector2>();
        }
    }

}

