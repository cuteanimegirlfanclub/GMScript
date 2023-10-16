using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Corotine Timer")]
    public class CorotineTimerSO : ScriptableObject
    {
        protected float currentTime = 0;
        public float timerSpeed = 1;
        public float CurrentTime { get => currentTime; }

        private Coroutine timerCoroutine;
        private bool isRunning = false;
        private bool timerCompleted = false;

        public event Action OnTimerBegin;
        public event Action OnTimerEnd;
        public event Action<float> OnTimerUpdate;

        public bool StartCount(MonoBehaviour mono)
        {
            if (!isRunning)
            {
                Debug.Log($"{this.name} is Running");
                timerCoroutine = mono.StartCoroutine(TimerCoroutine());
                timerCompleted = false;
                return true;
            }
            return false;
        }

        public bool StopCount(MonoBehaviour mono)
        {
            if (isRunning)
            {
                mono.StopCoroutine(timerCoroutine);
                isRunning = false;
                timerCompleted = true;
                return true;
            }
            return false;
        }

        public bool TimerCompleted()
        {
            return timerCompleted;
        }

        private IEnumerator TimerCoroutine()
        {
            currentTime = 0f;
            OnTimerBegin?.Invoke();

            while (currentTime < 10f)
            {
                currentTime += timerSpeed * Time.deltaTime;
                OnTimerUpdate?.Invoke(currentTime);
                yield return null;
            }

            OnTimerEnd?.Invoke();

            isRunning = false;
            timerCompleted = true;
        }
    }
}