using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Items/BareHandSO")]
    public class BareHandSO : SingleItemSO
    {
        public override byte[] BufferSOData()
        {
            return new byte[0];
        }

        public override void GetSOData(byte[] buffer)
        {

        }
    }
}
