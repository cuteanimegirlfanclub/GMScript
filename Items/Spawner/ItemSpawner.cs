using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class ItemSpawner : IDisposable
    {
        [SerializeField] private GameObject itemPrefab;
        private GameObject itemGO;

        public void InstantiateItem(GameObject prefab,Vector3 position, Quaternion rotaion)
        {
            itemPrefab = prefab;
            itemGO = GameObject.Instantiate(itemPrefab,position, rotaion);
            itemGO.GetComponent<BaseItemMono>().baseItemSO.gameObjectReference = itemGO;
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
