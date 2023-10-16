using UnityEngine;
using UnityEngine.SceneManagement;

namespace GMEngine
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();

                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                        go.AddComponent<SimpleStroage>();
                        go.tag = "Manager";
                    }
                }
                return _instance;
            }
        }
        public bool IsPause { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }


        public void PauseGame()
        {
            if(!IsPause) { Time.timeScale = 0; IsPause = true; }
        }

        public void ResumeGame()
        {
            if (IsPause) { Time.timeScale = 1; IsPause = false; }
        }

        public void PauseGameGate()
        {
            if (IsPause) { ResumeGame(); }
            else PauseGame();
        }

        public void PauseGame(bool toPause)
        {
            if (toPause) { PauseGame(); }
            else ResumeGame();
        }

        public async void SimpleLoadGameAsync(string savePath)
        {
            Scene current = SceneManager.GetActiveScene();

            await LevelManager.Instance.LoadScene(current.name);

            GetComponent<SimpleStroage>().SimpleLoad(savePath);
        }


        public void ReloadGameAtSlot(int slotNumber)
        {
            //ReloadGame(simpleStorage.TransferSlotToPath(slotNumber));
        }
    }

}

