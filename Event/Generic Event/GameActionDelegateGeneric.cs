using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GMEngine
{
    public abstract class GameActionDelegate<T> : ScriptableObject
    {
        private readonly List<Action<T>> @event = new List<Action<T>>();

        public void Raise(T t)
        {
            foreach (var action in @event) action.Invoke(t);
        }

        public void Subscribe(Action<T> action)
        {
            @event.Add(action);
        }

        public void UnSubscribe(Action<T> action)
        {
            @event.Remove(action);
        }
    }
}
