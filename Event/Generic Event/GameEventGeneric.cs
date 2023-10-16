using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Event.Generic
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        private List<IEventListener<T>> listeners = new List<IEventListener<T>>();

        public void Raise(T t)
        {
            for(int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(t);
            }
        }

        public void RegisterListener(IEventListener<T> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(IEventListener<T> listener)
        {
            listeners.Remove(listener);
        }
    }

}

