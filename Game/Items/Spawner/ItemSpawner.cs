using System;
using UnityEngine;


namespace GMEngine.Game
{
    public class ItemSpawner : IDisposable
    {
        [SerializeField] private GameObject itemPrefab;
        private GameObject itemGO;

        public void InstantiateItem(GameObject prefab,Vector3 position, Quaternion rotaion)
        {
            itemPrefab = prefab;
            //itemGO = ItemManager.Instance.CreateItemAsync(prefab.name, position, rotaion).GetAwaiter().GetResult();
            itemGO = GameObject.Instantiate(itemPrefab,position, rotaion);
        }

        public void Dispose()
        {
            ItemManager.Instance.PoolSpawner(this);
        }

        public void Reset()
        {
            itemPrefab = null;
            itemGO = null;
        }
    }

}
