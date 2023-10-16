using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace GMEngine
{
    public abstract class TimerSO : ScriptableObject
    {
        protected float currentTime = 0;
        public float timerSpeed = 1;
        public float CurrentTime { get => currentTime; }

        protected UniTaskCompletionSource<bool> timerCompletionSource;
        protected CancellationTokenSource cancellationTokenSource;

        protected abstract UniTask TimerStartCount();

        public abstract UniTask<bool> StartCountAsync();

        public virtual void ResetTimer()
        {
            currentTime = 0;

            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Dispose();
            }

        }

        protected void Reset()
        {
            currentTime = 0;
        }
    }
}

