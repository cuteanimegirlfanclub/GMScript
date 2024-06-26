using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GMEngine.Event;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using GMEngine.UI;
using System;
using UnityEngine.UIElements;

namespace GMEngine {

    public class SimpleStroage : MonoBehaviour
    {
        [Header("Debug")]
        public string savePath;
        public List<string> files = new List<string>();

        private SendGameDataEvent sendGameDataEvt;
        private ReceiveSaveDataEvent receiveDataEvt;

        public KeyCode saveKey = KeyCode.L;
        public KeyCode loadKey = KeyCode.K;

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "saveFile");
            GetSaveFiles();
            if(sendGameDataEvt == null)
            {
                sendGameDataEvt = ScriptableObject.CreateInstance<SendGameDataEvent>();
            }


            if(receiveDataEvt == null)
            {
                receiveDataEvt = ScriptableObject.CreateInstance<ReceiveSaveDataEvent>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(saveKey)) { Save(savePath); }
            if (Input.GetKeyUp(loadKey)) { Load(savePath); }
        }

        public async UniTask RegisterSendEvtListener(IGameDataSender listener)
        {
            if(sendGameDataEvt == null)
            {
               await UniTask.Yield();
            }
            else
            {
                sendGameDataEvt.RegisterListener(listener);
            }
        }

        public async UniTask RegisterReceiveEvtListener(ISaveDataRecevier listener)
        {
            if (receiveDataEvt == null)
            {
                await UniTask.Yield();
            }
            else
            {
                receiveDataEvt.RegisterListener(listener);
            }
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

        private async UniTask Save(string savePath)
        {
            GameManager.Instance.PauseGame(true);
            try
            {
                SuspendedConfirmationBox box = GlobalUIManager.GetSystemBox();
                if (await box.WaitForUserResponse("Save Current Data", BoxType.confirmation))
                {

                    using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
                    using (var Gwriter = new GameDataWriter(writer))
                    using (var data = new SaveData())
                    {
                        // Pulling data from game object to the game data class
                        RaiseOnPullDataRequest(data);
                        // Save game data to the file
                        data.Save(Gwriter);
                    }
                    GameManager.Instance.PauseGame(false);
                    Debug.Log($"{name} Request Successed");
                    return;
                }

            }
            catch (OperationCanceledException ex)
            {
                Debug.Log("Save operation was canceled: " + ex.Message);
            }
            finally
            {
                GameManager.Instance.PauseGame(false);
            }
        }

        private async UniTask Load(string savaPath)
        {
            GameManager.Instance.PauseGame(true);
            try
            {
                SuspendedConfirmationBox box = GlobalUIManager.GetSystemBox();
                if (await box.WaitForUserResponse("Load Latest Save File", BoxType.confirmation))
                {
                    await PerformLoadAsync(savePath);
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log("Load operation was canceled: " + ex.Message);
            }
            finally
            {
                GameManager.Instance.PauseGame(false);
            }
        }
        private async UniTask PerformLoadAsync(string savePath)
        {
            GlobalUIManager.SetLoadingUI(true);
            await SimpleLoadGameAsync(savePath);
            GlobalUIManager.SetLoadingUI(false);
        }

        public async UniTask SimpleLoadGameAsync(string savePath)
        {
            Scene current = SceneManager.GetActiveScene();
            await SceneManager.LoadSceneAsync(current.name, LoadSceneMode.Single);

            var state = GameManager.Instance.GetComponent<LifeCycleTracker>();
            while (state.currentState != LifeCycleTracker.GameState.Started)
            {
                await UniTask.Yield();
            }

            await SimpleLoad(savePath);
        }

        public async UniTask SimpleLoad(string savaPath)
        {
            using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
            using (var Greader = new SaveDataReader(reader))
            using (var data = new SaveData())
            {
                data.Load(Greader);
                await RaiseOnReceiveDataRequest(data);
            }
        }

        protected void RaiseOnPullDataRequest(SaveData data)
        {
            sendGameDataEvt.Raise(data);
        }

        protected async UniTask RaiseOnReceiveDataRequest(SaveData data)
        {
            await receiveDataEvt.Raise(data);
        }

        private void OnDestroy()
        {
            sendGameDataEvt.ClearListener();
            receiveDataEvt.ClearListener();
        }
    }
}

