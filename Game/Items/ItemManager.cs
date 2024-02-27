using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine
{
    public class ItemManager : Singleton<ItemManager>, ISaveDataRecevier
    {
        private ItemFactory factory;

        protected override void OnAwake()
        {
            factory = ScriptableObject.CreateInstance<ItemFactory>();
            var stroage = GameManager.Instance.GetComponent<SimpleStroage>();
            stroage.ReceiveDataEvt.RegisterListener(this);
        }

        public async UniTask ReceiveData(SaveData data)
        {
            Debug.Log($"Receiving Ground Item Data... {data.groundItemDatas.Count} Ground Items Intotal");
            //ground item data receiver
            foreach(var itemData in data.groundItemDatas)
            {
                Debug.Log(itemData.itemName);
                await CreateItemAsync(itemData.itemName, itemData.position, itemData.rotation);
            }
        }

        public async void CreateItem(string itemSOname, Vector3 position, Quaternion rotation)
        {
            await factory.CreateItemFromSOAsync(itemSOname, position, rotation);
        }

        public async UniTask<GameObject> CreateItemAsync(string itemSOname, Vector3 position, Quaternion rotation)
        {
            GameObject go = await factory.CreateItemFromSOAsync(itemSOname, position, rotation);
            return go;
        }

        public void PoolSpawner(ItemSpawner spawner)
        {
            factory.PoolSpawner(spawner);
        }

    }

}

