using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Input/InputSO")]
    public class InputSO : ScriptableObject
    {
        [SerializeField]
        private InputActionMap actions;

        public InputAction GetActionByName(string name)
        {
            return actions.FindAction(name);
        }
    }

}

