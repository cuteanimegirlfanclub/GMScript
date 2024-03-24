using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine.Game
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Property")]
        public FloatReferenceRW currentSpeed;
        public FloatReferenceRO runSpeed;
        public FloatReferenceRO walkSpeed;

        private Vector2 m_input;

        private CharacterController m_characterController;
        private Animator m_animator;

        [Header("Input Setting")]
        public InputAction moveAction;
        public InputAction runAction;

        private void Awake()
        {
            InitiatePlayerController();
        }
        private void OnEnable()
        {
            moveAction.Enable();
            runAction.Enable();

            currentSpeed.Value = 2.5f;
        }

        private void Update()
        {
            MovePlayer(m_input);
            RotatePlayer(m_input);
        }
        public void InitiatePlayerController()
        {
            m_characterController = GetComponent<CharacterController>();
            m_animator = GetComponent<Animator>();

            moveAction.performed += ReadVector2Input;
            moveAction.canceled += ReadVector2Input;

            runAction.performed += RunPlayer;
            runAction.canceled += WalkPlayer;
        }


        public void ReadVector2Input(InputAction.CallbackContext ctx)
        {
            m_input = ctx.ReadValue<Vector2>();
        }

        public void RunPlayer(InputAction.CallbackContext ctx)
        {
            currentSpeed.Value = runSpeed.Value;
        }
        public void WalkPlayer(InputAction.CallbackContext ctx)
        {
            currentSpeed.Value = walkSpeed.Value;
        }

        public void MovePlayer(Vector2 input)
        {
            m_animator.SetFloat("Vertical Speed", input.magnitude * currentSpeed.Value, 0.1f, Time.deltaTime);
        }

        public void RotatePlayer(Vector2 input)
        {
            Vector3 direction;
            direction = transform.InverseTransformDirection(new Vector3(input.x, 0f, input.y));

            float radius = Mathf.Atan2(direction.x, direction.z);
            m_animator.SetFloat("Turn Speed", radius, 0.1f, Time.deltaTime);
        }

        private void OnAnimatorMove()
        {
            m_characterController.SimpleMove(m_animator.velocity);
            transform.Rotate(m_animator.deltaRotation.eulerAngles);
        }
    }

}

