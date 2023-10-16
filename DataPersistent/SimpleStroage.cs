using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace GMEngine {

    public class SimpleStroage : MonoBehaviour
    {
        [Header("Debug")]
        public string savePath;
        public List<string> files = new List<string>();

        public event Action<SaveData> OnPullDataRequest;
        public event Action<SaveData> OnReceiveDataRequest;

        public KeyCode saveKey = KeyCode.L;
        public KeyCode loadKey = KeyCode.K;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "saveFile");
            GetSaveFiles();
        }

        private void Update()
        {
            if(Input.GetKeyDown(saveKey)) { Save(savePath); }
            if (Input.GetKeyDown(loadKey)) { Load(savePath); }
        }

        private void GetSaveFiles()
        {
            string dataPath = Application.persistentDataPath;

            if (Directory.Exists(dataPath))
            {
                string[] tempfiles = Directory.GetFiles(dataPath);

                files.AddRange(tempfiles);

                foreach (string file in files)
                {
                    Debug.Log("Found file: " + file);
                }
            }
        }

        private void Save(string savePath)
        {
            using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
            using (var Gwriter = new SaveDataWriter(writer))
            using (var data = new SaveData())
            {
                //pulling data from game object to the game data class
                OnPullDataRequest?.Invoke(data);
                //save game data to the file
                //data.AsyncSave(Gwriter);
                data.Save(Gwriter);
            }
        }

        private void Load(string savaPath)
        {
            GameManager.Instance.SimpleLoadGameAsync(savePath);
        }

        public void SimpleLoad(string savaPath)
        {
            using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
            using (var Greader = new SaveDataReader(reader))
            using (var data = new SaveData())
            {
                data.Load(Greader);
                OnReceiveDataRequest?.Invoke(data);
            }
        }
    }
}

