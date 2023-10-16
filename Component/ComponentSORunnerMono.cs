using System.Collections.Generic;
using UnityEngine;

namespace GMEngine{

    [DisallowMultipleComponent]
    public class ComponentSORunnerMono : MonoBehaviour
    {
        //maybe i should make a listreferenceso class?
        public List<ComponentSO> Components;

        private void Awake()
        {
            InitiateComponentSO();
        }

        public void InitiateComponentSO()
        {
            foreach (var component in Components) { component.AddComponent(gameObject); }
        }

        public void AddComponent(ComponentSO component)
        {
            if (!ComponentCheck(component))
            {
                Components.Add(component);
                component.AddComponent(gameObject);
            }
        }

        private bool ComponentCheck(ComponentSO component)
        {
            return Components.Contains(component);

        }
    }
}

