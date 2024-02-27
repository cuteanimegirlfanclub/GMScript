using GMEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
#if UNITY_EDITOR
    public class ItemSpawnerMono : MonoBehaviour
    {
        [Header("Editor Configure")]
        [SerializeField] public LevelConfigure configure;
        public GameObject prefab;

        private void OnValidate()
        {
            if (configure == null)
            {
                //level manager are singleton runtime
                LevelManager manager = FindObjectOfType<LevelManager>();
                if (manager != null)
                {
                    configure = manager.GetLevelConfigure();
                }
                else
                {
                    Debug.Log("cant find level manager!");
                }
            }
        }

        public void Bake()
        {
            configure.SetupSpawnerData(this);
        }
    }
#endif

}

