using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Behaviours/Human Rotate Behaviour")]
    public class HumanRotateBehaviour : InputBehaviourSO
    {
        public AnimatorSO animatorSO;

        public override void BindBehaviour(InputAction.CallbackContext context)
        {
            Vector3 direction = new Vector3(GetInputData(context).x, 0f, GetInputData(context).y);

            //direction =Transform.InverseTransformDirection(new Vector3(input.x, 0f, input.y));
            //direction = Transform.InverseTransformDirection(direction);
            float radius = Mathf.Atan2(direction.x, direction.z);
            animatorSO.animator.SetFloat("Turn Speed", radius, 0.1f, Time.deltaTime);
        }

        private Vector2 GetInputData(InputAction.CallbackContext context)
        {
            return context.ReadValue<Vector2>();
        }
    }

}
