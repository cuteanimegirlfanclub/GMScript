using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace GMEngine
{
    public class ItemBehaviour : MonoBehaviour
    {
        public InputAction action;
        public UnityEvent unityEvent;

        private void OnEnable()
        {
            action.Enable();
            action.canceled += ActionHandler;
        }

        private void OnDisable()
        {
            action.Disable();
            action.canceled -= ActionHandler;
        }

        private void ActionHandler(InputAction.CallbackContext ctx)
        {
            unityEvent.Invoke();
        }

        private void OnValidate()
        {
            if(action != null)
            {
                if(action.type != InputActionType.Button)
                {
                    Debug.LogError("Item Behaviour Should Have Button Action Input!");
                }
            }
        }
    }

}

