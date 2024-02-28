using UnityEngine;
using GMEngine.TransformExtension;
using Cysharp.Threading.Tasks;

namespace GMEngine
{
    public class PersistableItem : MonoBehaviour, IGameDataSender
    {
        private void Awake()
        {
            RegisterSendDataEvt();
        }

        private async UniTaskVoid RegisterSendDataEvt()
        {
            var st = GameManager.Instance.GetComponent<SimpleStroage>();
            await st.RegisterSendEvtListener(this);
        }

        public void SendData(SaveData data)
        {
            int state = transform.ItemStateCheck();
            if (state == 1)
            {
                Debug.Log("Sending ground item data...");
                data.groundItemDatas.Add(data.PackToSaveData(gameObject));
            }
        }

    }

}

