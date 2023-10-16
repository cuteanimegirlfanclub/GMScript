using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Event
{
    public abstract class GameEvent : ScriptableObject
    {
        private List<IEventListener> listeners = new List<IEventListener>();

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(IEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(IEventListener listener)
        {
            listeners.Remove(listener);
        }
    }

}

