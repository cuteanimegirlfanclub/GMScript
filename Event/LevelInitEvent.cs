using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class LevelInitEvent : ScriptableObject
    {
        private List<ILevelInitializer> initializers = new List<ILevelInitializer>();

        public void RegisterListener(ILevelInitializer initializer)
        {
            if(initializers.Contains(initializer)) return;
            initializers.Add(initializer);
        }

        public void UnregisterListener(ILevelInitializer initializer)
        {
            if (initializers.Contains(initializer))
            {
                initializers.Remove(initializer);
            }
        }

        public void Raise()
        {
            foreach (ILevelInitializer initializer in initializers)
            {
                initializer.Init();
            }
        }
    }

}

