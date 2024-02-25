using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMEngine
{
    public class SaveData : ScriptableObject, IDisposable
    {
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

        public void Save(GameDataWriter writer)
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
                WriteItemData(itemData, writer);
                WriteItemPosition(itemData, writer);
            }

            //Inventory Data
            writer.Write(inventoryItemDatas.Count);
            foreach (var itemData in inventoryItemDatas)
            {
                WriteItemData(itemData, writer);
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
                    int length = reader.ReadInt();
                    byte[] itemDataBuffer = reader.ReadBytes(length);

                    var itemPosition = reader.ReadVector3();
                    var itemRotation = reader.ReadQuaternion();


                    groundItemDatas.Add(new ItemData(name, itemDataBuffer, itemPosition, itemRotation));

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
                    int length = reader.ReadInt();
                    byte[] itemDataBuffer = reader.ReadBytes(length);


                    inventoryItemDatas.Add(new ItemData(name,itemDataBuffer, new Vector3(0, -5, 0), Quaternion.identity));
                }
            }
            handItemNum = reader.ReadInt();

        }

        private void WriteItemData(ItemData itemData, GameDataWriter writer)
        {

            writer.Write(itemData.itemName);
            writer.Write(itemData.itemDataBuffer.Length);
            writer.Write(itemData.itemDataBuffer);
        }

        private void WriteItemPosition(ItemData itemData, GameDataWriter writer)
        {
            writer.Write(itemData.position);
            writer.Write(itemData.rotation);
        }


        public ItemData PackToSaveData(GameObject gameObject)
        {
            var itemSO = gameObject.GetComponent<PickableItem>().baseItemSO;
            string name = itemSO.GetType().Name;
            byte[] buffer = itemSO.BufferSOData();
            Vector3 position = gameObject.transform.position;
            quaternion rotation = gameObject.transform.rotation;

            return new ItemData(name, buffer, position, rotation);
        }
    }

    [Serializable]
    public class ItemData
    {
        public BaseItemSO baseItemSO;
        /// <summary>
        /// In the form of SO
        /// </summary>
        public byte[] itemDataBuffer;
        public string itemName;
        public Vector3 position;
        public Quaternion rotation;

        public ItemData(string name, byte[] buffer, Vector3 position, Quaternion rotarion)
        {
            this.itemName = name;
            this.itemDataBuffer = buffer;
            this.position = position;
            this.rotation = rotarion;
        }
    }
}

