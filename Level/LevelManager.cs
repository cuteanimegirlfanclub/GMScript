using GMEngine.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using GMEngine.StringExtension;
using UnityEditor;

namespace GMEngine
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        public LevelConfigure configure;
        public SendGameDataEvent sendLevelDataEvt;

        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                sendLevelDataEvt = ScriptableObject.CreateInstance<SendGameDataEvent>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async void LoadSceneAsync(string sceneName)
        {
            GlobalUIManager.Instance.SetLoadingUI(true);
            await Addressables.LoadSceneAsync(sceneName);
            LevelConfigure configure = await Addressables.LoadAssetAsync<LevelConfigure>(sceneName.SceneToConfigure());
            this.configure = configure;
            await configure.SetupLevel(); 
            GlobalUIManager.Instance.SetLoadingUI(false);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }


        public async UniTask SetupLevel()
        {
            if(configure == null)
            {
                if(SceneManager.GetActiveScene().buildIndex == 0)
                {
                    return;
                }
                return;
            }

            await configure.SetupLevel();
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

