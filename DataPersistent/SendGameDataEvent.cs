
using UnityEngine;

namespace GMEngine.Event
{
    public class SendGameDataEvent : GameEvent<IGameDataSender>
    {
        public void Raise(SaveData saveData)
        {
            foreach(var listener in listeners)
            {
                listener.SendData(saveData);
            }
        }
    }

}

