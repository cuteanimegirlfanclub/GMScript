using GMEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GMEngine
{
    public class GameManager : Singleton<GameManager>
    {
        public bool isPause;

        protected override void OnAwake()
        {
            //gameObject.AddComponent<SimpleStroage>();
            //gameObject.AddComponent<LifeCycleTracker>();
        }


        public void PauseGame()
        {
            if(!isPause) { Time.timeScale = 0; isPause = true; }
        }

        public void ResumeGame()
        {
            if (isPause) { Time.timeScale = 1; isPause = false; }
        }

        public void PauseGameGate()
        {
            if (isPause) { ResumeGame(); }
            else PauseGame();
        }

        public void PauseGame(bool toPause)
        {
            if (toPause) { PauseGame(); }
            else ResumeGame();
        }
    }

}

