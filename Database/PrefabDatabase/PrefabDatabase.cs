using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Database/PrefabDatabase")]
    public class PrefabDatabase : ScriptableObject, IAssetDatabase<GameObject>
    {
        [SerializeField]
        private List<GameObject> prefabs;

        public ICollection<GameObject> Assets
        {
            get => prefabs;
        }
        public GameObject GetPrefabAtIndex(int index)
        {
            if (index >= 0 && index < prefabs.Count)
            {
                return prefabs[index];
            }
            else
            {
                Debug.LogWarning($"Invalid index: {index}. No prefab found.");
                return null;
            }
        }
        public GameObject GetPrefabByName(string name)
        {
            return prefabs.Find(prefab => prefab.name == name);
        }
    }
}

