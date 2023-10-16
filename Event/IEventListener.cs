using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public interface IEventListener
    {
        public void OnEventRaised();
    }

    public interface IEventListener<T>
    {
        public void OnEventRaised(T t);
    }

}

