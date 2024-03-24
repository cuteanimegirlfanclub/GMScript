using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine.GameObjectExtension
{
    public static class GameObjectExtension
    {
        public static void FactoryReset(this GameObject gameObject)
        {
            //Note that messages will not be sent to inactive objects
            gameObject.SendMessage("FactoryReset");
        }

        public static int ActiveChildCount(this GameObject gameObject)
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


        public static bool TryFindGameObjectWithTag(string tag, out GameObject gameObject)
        {
            gameObject = GameObject.FindWithTag(tag);
            if (gameObject)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ItemGOBehaviour(this GameObject gameObject, bool key)
        {
            gameObject.GetComponent<ItemBehaviour>().enabled = key;
        }

        public static async UniTask<T> AwaitGetComponent<T>(this GameObject gameObject) where T : Component
        {
            await UniTask.WaitUntil(() => gameObject.TryGetComponent(out T t));
            return gameObject.GetComponent<T>();
        }

        public static async UniTask<GameObject> AwaitFindGameObjectWithTag(this GameObject gameObject, string tag)
        {
            await UniTask.WaitUntil( () => TryFindGameObjectWithTag(tag , out GameObject gameObject1));
            return GameObject.FindGameObjectWithTag(tag);
        }
    }
}

