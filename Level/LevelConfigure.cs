using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UIElements;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Level/Level Configure")]
    public class LevelConfigure : ScriptableObject
    {
        public int mapBuildIndex;

        //the spawners name here is in the form of GO
        public List<ItemData> spawners = new List<ItemData>();

        public async void SetupLevel()
        {
            foreach(ItemData item in spawners)
            {
                await ItemManager.Instance.CreateItemAsync(item.name, item.position, item.rotation);
            }
        }

#if UNITY_EDITOR
        [SerializeField]
        public Dictionary<GameObject, int> editorSpawners = new Dictionary<GameObject, int>();
        private void OnValidate()
        {

        }

        public void SetupSpawnerData(ItemSpawnerMono spawner)
        {
            if (SameSpawner(spawner))
            {
                ReplaceSpawnerData(spawner);
            }
            else
            {
                AddSpawnerData(spawner);
            }
        }
        private bool SameSpawner(ItemSpawnerMono spawner)
        {
            return editorSpawners.ContainsKey(spawner.gameObject);
        }

        private void ReplaceSpawnerData(ItemSpawnerMono spawner)
        {
            string name = spawner.prefab.name;
            Vector3 position = spawner.transform.position;

            int index = spawners.FindIndex(itemData => itemData.position == position && itemData.name == name);

            if (index != -1)
            {
                ItemData newData = new ItemData(name, null, position, Quaternion.identity);
                spawners[index] = newData;
            }
            else
            {
                Debug.Log("No matching spawner found to replace, Adding New ItemData");
            }
        }

        private void AddSpawnerData(ItemSpawnerMono spawner)
        {
            string name = spawner.prefab.name;
            Vector3 position = spawner.transform.position;
            Quaternion rotation = Quaternion.identity;

            if (spawners.Any(itemData => itemData.position == position))
            {
                throw new InvalidOperationException("Item with the same position already exists.");
            }

            ItemData data = new ItemData(name, null, position, rotation);
            spawners.Add(data);
            editorSpawners.Add(spawner.gameObject, spawners.IndexOf(data));
        }
#endif
    }
}


