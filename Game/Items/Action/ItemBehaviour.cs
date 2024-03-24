using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace GMEngine
{
    public class ItemBehaviour : MonoBehaviour
    {
        public InputAction input;
        public UnityEvent _event;

        public UnityAction evtDelegate;
        public string actionName;

        private void Awake()
        {
            evtDelegate += InvokEvt;
        }

        private void OnEnable()
        {
            input.Enable();
            input.canceled += ActionHandler;
            
        }

        private void OnDisable()
        {
            input.Disable();
            input.canceled -= ActionHandler;
        }

        private void ActionHandler(InputAction.CallbackContext ctx)
        {
            InvokEvt();
        }

        private void InvokEvt()
        {
            _event.Invoke();
        }

        private void OnValidate()
        {
            if(input != null)
            {
                if(input.type != InputActionType.Button)
                {
                    Debug.LogError("Item Behaviour Should Have Button Action Input!");
                }
            }
        }
    }

}

