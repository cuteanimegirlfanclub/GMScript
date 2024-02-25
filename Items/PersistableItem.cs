using UnityEngine;
using GMEngine.TransformExtension;

namespace GMEngine
{
    public class PersistableItem : MonoBehaviour, IGameDataSender
    {
        private void Awake()
        {
            RegisterSendDataEvt();
        }

        private void RegisterSendDataEvt()
        {
            var st = GameManager.Instance.GetComponent<SimpleStroage>();
            st.SendGameDataEvt.RegisterListener(this);
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

