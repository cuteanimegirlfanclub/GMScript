using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine
{
    public class ItemManager : MonoBehaviour, ISaveDataRecevier
    {
        private static ItemManager _instance;
        private ItemFactory factory;

        public static ItemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ItemManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("ItemManager");
                        _instance = go.AddComponent<ItemManager>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            factory = ScriptableObject.CreateInstance<ItemFactory>();
            var stroage = GameManager.Instance.GetComponent<SimpleStroage>();
            stroage.OnReceiveDataRequest += ReceiveData;
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void ReceiveData(SaveData data)
        {
            Debug.Log("Receiving Ground Item Data...");
            Debug.Log(data.groundItemDatas.Count);
            //ground item data receiver
            foreach(var itemData in data.groundItemDatas)
            {
                Debug.Log(itemData.name);
                CreateItem(itemData.name, itemData.position, itemData.rotation);
            }
        }

        public void CreateItem(string itemSOname, Vector3 position, Quaternion rotation)
        {
            factory.CreateItemFromSOAsync(itemSOname, position, rotation);
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

