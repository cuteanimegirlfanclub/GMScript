using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Value
{
    [Serializable]
    public abstract class ValueReference
    {
        public abstract ScriptableObject GetVariableSO();
    }

    public abstract class ValueReferenceRO : ValueReference
    {

    }

    public abstract class ValueReferenceRW : ValueReference
    {

    }
}

