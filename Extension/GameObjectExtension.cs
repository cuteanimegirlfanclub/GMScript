using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GameObjectExtension
{
    public static class GameObjectExtension
    {
        public static void FactoryReset(this GameObject gameObject)
        {
            //Note that messages will not be sent to inactive objects
            gameObject.SendMessage("FactoryReset");
        }

        public static int GetActiveChildCount(this GameObject gameObject)
        {
            int activeChildCount = 0;
            foreach (GameObject child in gameObject.transform)
            {
                if (child.activeSelf)
                {
                    activeChildCount++;
                }
            }
            return activeChildCount;
        }
    }
}

