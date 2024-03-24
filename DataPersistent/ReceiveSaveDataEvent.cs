using Cysharp.Threading.Tasks;
using GMEngine.Event;
using System;
using UnityEngine;

namespace GMEngine
{
    public class ReceiveSaveDataEvent : GameEvent<ISaveDataRecevier>
    {
        public async UniTask Raise(SaveData saveData)
        {
            foreach (var listener in listeners)
            {
                Debug.Log(listener.ToString());
                try
                {
                    await listener.ReceiveData(saveData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception in ReceiveData: {ex.Message}");
                }
            }
        }

    }
}

