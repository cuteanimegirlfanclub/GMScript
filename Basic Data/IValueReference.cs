using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Value
{
    public interface IValueReference
    {
        public abstract ScriptableObject GetVariableSO();
    }

    public interface IValueReferenceRO : IValueReference
    {

    }

    public interface IValueReferenceRW : IValueReference
    {

    }
}

