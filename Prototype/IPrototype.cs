using UnityEngine;

namespace GMEngine.Prototype
{
    public interface IPrototype<T>
    {
        public T DeepCopy();
    }
}

