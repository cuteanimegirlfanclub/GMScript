using UnityEngine;

namespace GMEngine.TransformExtension
{
    public static class TransformExtension
    {
        /// <summary>
        /// 0 -> InInventory; 1 -> OnGround;  2 -> OnHand
        /// </summary>
        /// <returns></returns>
        public static int ItemStateCheck(this Transform transform)
        {
            if (transform.position.y < 0) return 0;
            else if (transform.parent != null) return 2; else return 1;
        }

    }

}

namespace GMEngine.Editor.TransformExtension
{
}

