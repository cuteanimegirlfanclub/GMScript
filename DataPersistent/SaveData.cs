using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMEngine
{
    public class SaveData : ScriptableObject, IDisposable
    {
        //
        public int saveVersion;

        //Scene Data
        public int sceneBuildIndex;

        //Player Data
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public float playerHealth;
        public float playerMental;

        //Item Data
        public List<ItemData> groundItemDatas = new List<ItemData>();



        //Inventory Data
        public List<ItemData> inventoryItemDatas = new List<ItemData>();
        public int handItemNum;




        //Monster Data






        public void Dispose()
        {
            Destroy(this);
        }

        public void Save(SaveDataWriter writer)
        {
            //player Date
            sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            writer.Write(sceneBuildIndex);
            writer.Write(playerPosition);
            writer.Write(playerRotation);

            writer.Write(playerHealth);
            writer.Write(playerMental);

            //Item Data
            writer.Write(groundItemDatas.Count);
            foreach (var itemData in groundItemDatas)
            {
                writer.Write(itemData.name);
                itemData.baseItemSO.WriteSpecial(writer);

                writer.Write(itemData.position);
                writer.Write(itemData.rotation);

                if (itemData.baseItemSO is StackableItemSO stackableItem)
                {
                    writer.Write(stackableItem.number);
                }
            }

            //Inventory Data
            writer.Write(inventoryItemDatas.Count);
            foreach (var itemData in inventoryItemDatas)
            {
                writer.Write(itemData.name);
                itemData.baseItemSO.WriteSpecial(writer);
            }
            writer.Write(handItemNum);
        }

        public void Load(SaveDataReader reader)
        {
            //Player Data
            sceneBuildIndex = reader.ReadInt();
            playerPosition = reader.ReadVector3();
            playerRotation = reader.ReadQuaternion();

            playerHealth = reader.ReadFloat();
            playerMental = reader.ReadFloat();

            //Item Data
            groundItemDatas.Clear();
            int itemNum = reader.ReadInt();
            if (itemNum > 0) 
            {
                for (int i = 0; i < itemNum; i++)
                {
                    string name = reader.ReadString();
                    BaseItemSO itemSO = CreateInstance(name) as BaseItemSO;
                    itemSO.ReadSpecial(reader);

                    var itemPosition = reader.ReadVector3();
                    var itemRotation = reader.ReadQuaternion();

                    if (itemSO is StackableItemSO stackableItem)
                    {
                        stackableItem.number = reader.ReadFloat();
                        groundItemDatas.Add(new ItemData(name, stackableItem, itemPosition, itemRotation));

                    }

                    groundItemDatas.Add(new ItemData(name, itemSO, itemPosition, itemRotation));

                }
            }
            

            //Inventory Data
            inventoryItemDatas.Clear();
            int inventoryNum = reader.ReadInt();
            Debug.Log($"Loading {inventoryNum} Inventory Items");
            if (inventoryNum > 0)
            {
                for (int i = 0; i < inventoryNum; i++)
                {
                    string name = reader.ReadString();
                    BaseItemSO itemSO = ScriptableObject.CreateInstance(name) as BaseItemSO;
                    itemSO.ReadSpecial(reader);

                    inventoryItemDatas.Add(new ItemData(name, itemSO, new Vector3(0, -5, 0), Quaternion.identity));
                }
            }
            handItemNum = reader.ReadInt();

        }

        public ItemData PackToSaveData(GameObject gameObject)
        {
            var itemSO = gameObject.GetComponent<BaseItemMono>().baseItemSO;
            string name = itemSO.GetType().Name;
            Vector3 position = gameObject.transform.position;
            quaternion rotation = gameObject.transform.rotation;

            return new ItemData(name, itemSO, position, rotation);
        }

        public BaseItemSO UnPackToGameData(ItemData itemData)
        {
            return itemData.baseItemSO;
        }

#if UNITY_EDITOR
        public void LogSaveData()
        {

        }
#endif
    }

    [Serializable]
    public class ItemData
    {
        public BaseItemSO baseItemSO;
        /// <summary>
        /// In the form of SO
        /// </summary>
        public string name;
        public Vector3 position;
        public Quaternion rotation;

        public ItemData(string name, BaseItemSO baseItemSO, Vector3 position, Quaternion rotarion)
        {
            this.name = name;
            this.baseItemSO = baseItemSO;
            this.position = position;
            this.rotation = rotarion;
        }
    }
}

