using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;



namespace GMEngine
{
    public class ItemFactory : ScriptableObject
    {
        private Stack<ItemSpawner> spawnerStack = new Stack<ItemSpawner>();
        private const int MaxSpawners = 10;

        public async UniTask<GameObject> CreateItemFromSOAsync(string itemSOName, Vector3 position, Quaternion rotation)
        {
            string name = ConvertSOToGO(itemSOName);

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                ItemSpawner spawner = GetSpawner();
                spawner.InstantiateItem(handle.Result, position, rotation);
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

        public void PoolSpawner(ItemSpawner spawner)
        {
            spawner.Reset();

            spawnerStack.Push(spawner);
        }

    }
}

