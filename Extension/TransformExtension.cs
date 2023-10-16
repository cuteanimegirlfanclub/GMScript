using UnityEngine;

namespace GMEngine.TransformExtension
{
    public static class TransformExtension
    {
        public static Transform HoldRotation(this Transform transfrom)
        {
            Quaternion holdingRotation = transfrom.rotation;
            transfrom.rotation = holdingRotation;
            return transfrom;
        }
    }

}

namespace GMEngine.TransformExtension.UI
{
    public static class UITrExtension
    {
        //public static Transform 
    }
}

