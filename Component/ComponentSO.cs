using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public abstract class ComponentSO : ScriptableObject
    {
        public abstract void AddComponent(GameObject gameObject);
    }
}

