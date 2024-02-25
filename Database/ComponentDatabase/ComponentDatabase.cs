using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Database/ComponentDatabase")]
    public class ComponentDatabase : ScriptableObject, IAssetDatabase<Component>
    {
        [SerializeField]
        private List<Component> m_Components;
        public ICollection<Component> Assets => m_Components;
    }

}

