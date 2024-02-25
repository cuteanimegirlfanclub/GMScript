using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public interface IAssetDatabase<T> where T : Object
    {
        public ICollection<T> Assets { get; }
    }

}

