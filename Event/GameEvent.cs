using System.Collections.Generic;
using UnityEngine;
using System;

namespace GMEngine.Event
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        protected List<T> listeners = new List<T>();

        public void RegisterListener(T listener)
        {
            if(listeners.Contains(listener)) return;
            listeners.Add(listener);
        }

        public void UnregisterListener(T listener)
        {
            listeners.Remove(listener);
        }

        public void ClearListener()
        {
            listeners.Clear();
        }
    }
}

