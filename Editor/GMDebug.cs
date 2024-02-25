using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Editor
{
    public static class GMDebug
    {
        public static void CheckNull(this Debug debug, object obj)
        {
            if (obj == null)
            {
                Debug.Log($"{obj.ToString()} is Null");
            }
            else
            {
                Debug.Log($"{obj.ToString()} Not Null");
            }
        }
    }

}

