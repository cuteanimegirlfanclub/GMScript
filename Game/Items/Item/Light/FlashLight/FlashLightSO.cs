using GMEngine.Value;
using System;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Items/FlashLightSO")]
    public class FlashLightSO : SingleItemSO
    {
        public FloatVariable currentCharge;
        public override byte[] BufferSOData()
        {
            return BitConverter.GetBytes(currentCharge.Value);
        }

        public override void GetSOData(byte[] buffer)
        {
            currentCharge.SetValue(BitConverter.ToSingle(buffer, 0));
        }
    }

}

