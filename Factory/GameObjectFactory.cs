using System.Collections.Generic;
using UnityEngine;
using GMEngine.GameObjectExtension;

namespace GMEngine
{
    public class GameObjectFactory : ScriptableObject
    {
        private Stack<GameObject> stack = new Stack<GameObject>();
        public int maxPool;
        public GameObject gameObjectPrefab;
        private readonly object factoryLock = new object();

        private int totalCreated = 0;

        public GameObject GetProduct(Transform parent)
        {
            lock (factoryLock)
            {
                Debug.Log($"Getting Product {gameObjectPrefab.name}, Factory Now have {stack.Count} Product");
                if (stack.Count > 0)
                {
                    Debug.Log($"Find Pooled Product {gameObjectPrefab.name}");
                    GameObject gameObject = stack.Pop();
                    gameObject.SetActive(true);
                    return gameObject;
                }

                if (stack.Count < maxPool)
                {
                    Debug.Log($"Creating New Product {gameObjectPrefab.name} Parent is {parent}");
                    GameObject newProduct = Instantiate(gameObjectPrefab, parent);
                    newProduct.name = totalCreated++.ToString();
                    return newProduct;
                }
                return null;
            }
        }

        public bool TryGetProduct(Transform parent, out GameObject product)
        {
            if (stack.Count > 0)
            {
                product = stack.Pop();
                product.SetActive(true);
                return true;
            }

            if (stack.Count < maxPool)
            {
                product = Instantiate(gameObjectPrefab, parent);
                return true;
            }
            product = null;
            Destroy(product);
            return false;
        }

        public void PoolProduct(GameObject product)
        {
            stack.Push(product);
            product.FactoryReset();
            product.SetActive(false);
            Debug.Log($"Pooling Product, Now we have {stack.Count} Products");
        }

        public void Setup(GameObject prefab, int maxPool)
        {
            gameObjectPrefab = prefab;
            this.maxPool = maxPool;
        }

#if UNITY_EDITOR
        public void OnEnable()
        {
            //    if(gameObjectPrefab == null) { Debug.LogWarning($"FactoryGOPrefab at {name} not assign!"); }
            //    if(maxPool == 0) { Debug.LogWarning($"FactoryGO maxPool at {name} Setting not assign!"); }
        }
#endif
    }

}

