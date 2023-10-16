using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Events/Game Action Delegate")]
    public class GameActionDelegate : ScriptableObject 
    {
        private readonly List<Action> @event = new List<Action>();

        public void Raise()
        {
            foreach (var action in @event) action.Invoke();
        }

        public void Subscribe(Action action)
        {
            @event.Add(action);
        }

        public void UnSubscribe(Action action)
        {
            @event.Remove(action);
        }
    }
}

