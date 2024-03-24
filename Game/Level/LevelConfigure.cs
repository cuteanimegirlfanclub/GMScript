using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using GMEngine.Event;
using Cysharp.Threading.Tasks;

namespace GMEngine.Game
{
    [CreateAssetMenu(menuName = "Scriptable Object/Level/Level Configure")]
    public class LevelConfigure : ScriptableObject
    {
        public int mapBuildIndex;

        //the spawners name here is in the form of GO
        [SerializeField]
        private List<ItemData> levelItemDatas = new List<ItemData>();
        public ICollection<ItemData> Assets => levelItemDatas;

        public async UniTask SetupLevel()
        {
            foreach(ItemData item in levelItemDatas)
            {
                await ItemManager.Instance.CreateItemAsync(item.itemName, item.position, item.rotation);
            }
        }

        public void RegisterLevelData()
        {
            levelItemDatas.Clear();
        }

#if UNITY_EDITOR
        [SerializeField]
        public Dictionary<GameObject, int> editorSpawners = new Dictionary<GameObject, int>();


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

            int index = levelItemDatas.FindIndex(itemData => itemData.position == position && itemData.itemName == name);

            if (index != -1)
            {
                ItemData newData = new ItemData(name, null, position, Quaternion.identity);
                levelItemDatas[index] = newData;
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

            if (levelItemDatas.Any(itemData => itemData.position == position))
            {
                throw new InvalidOperationException("Item with the same position already exists.");
            }

            ItemData data = new ItemData(name, null, position, rotation);
            levelItemDatas.Add(data);
            editorSpawners.Add(spawner.gameObject, levelItemDatas.IndexOf(data));
        }
#endif
    }
}


