using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GMEngine
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [SerializeField] private LevelConfigure configure;

        [SerializeField]
        private GameObject loadingUICanvas;
        [SerializeField]
        private Slider sliderBar;

        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                SetupLevel();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async Task LoadScene(string sceneName)
        {
            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            loadingUICanvas.SetActive(true);

            do
            {
                await Task.Delay(100);
                sliderBar.value = sceneOperation.progress;
            } while (!sceneOperation.isDone);

            loadingUICanvas.SetActive(false);
        }

        public void SetupLevel()
        {
            configure.SetupLevel();
        }

#if UNITY_EDITOR
        public LevelConfigure GetLevelConfigure()
        {
            if (configure == null) {
                Debug.Log("no registered level configure");
                return null; } 
            return configure;
        }

        private void OnValidate()
        {
            if(configure != null)
            {
                configure.mapBuildIndex = SceneManager.GetActiveScene().buildIndex;
            }
        }
#endif
    }

}

