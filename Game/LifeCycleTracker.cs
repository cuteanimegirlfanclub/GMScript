using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace GMEngine
{
    public class LifeCycleTracker : MonoBehaviour
    {
        public enum GameState 
        {
            Initializing,
            Started,
        }

        public GameState currentState;

        private void Awake()
        {
            currentState = GameState.Initializing;
        }

        private void OnEnable()
        {
            currentState = GameState.Started;
        }
    }

}

