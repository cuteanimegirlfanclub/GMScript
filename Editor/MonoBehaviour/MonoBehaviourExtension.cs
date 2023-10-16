using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.MonobehaviourExtension
{
    public static class MonoBehaviourExtension
    {
        public static MonoBehaviour NoActiveOnBeginning(this MonoBehaviour behaviour)
        {
            behaviour.enabled = false;
            return behaviour;
        }
    }

}

