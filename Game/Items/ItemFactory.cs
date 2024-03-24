using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


namespace GMEngine.Game
{
    public class ItemFactory : ScriptableObject
    {
        private Stack<ItemSpawner> spawnerStack = new Stack<ItemSpawner>();
        private const int MaxSpawners = 10; 
        private readonly object spawnerLock = new object();


        public async UniTask<GameObject> CreateItemFromSOAsync(string itemSOName, Vector3 position, Quaternion rotation)
        {
            string name = ConvertSOToGO(itemSOName);
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
            await handle.ToUniTask();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                ItemSpawner spawner = GetSpawner();
                spawner.InstantiateItem(handle.Result, position, rotation);
                Debug.Log($"Item {handle.Result} Creation Succeed");
                return handle.Result;
            }
            else
            {
                Debug.LogError("Failed to load item: " + handle.OperationException);
                return null;
            }
        }

        private string ConvertSOToGO(string input)
        {
            string result = input.Replace("SO", "GO");
            return result;
        }

        private ItemSpawner GetSpawner()
        {
            lock (spawnerLock)
            {
                if (spawnerStack.Count > 0)
                {
                    return spawnerStack.Pop();
                }

                if (spawnerStack.Count < MaxSpawners)
                {
                    ItemSpawner newSpawner = new ItemSpawner();
                    return newSpawner;
                }

                return null;
            }
        }

        public void PoolSpawner(ItemSpawner spawner)
        {
            spawner.Reset();

            spawnerStack.Push(spawner);
        }

    }
}

