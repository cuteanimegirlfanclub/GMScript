using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Simple Timer")]
    public class SimpleTimerSO : TimerSO
    {
        public event Action OnTimerBegin;
        public event Action OnTimerEnd;
        public event Action<float> OnTimerUpdate;

        protected override async UniTask TimerStartCount()
        {
            currentTime = 0;
            cancellationTokenSource = new CancellationTokenSource();


            OnTimerBegin?.Invoke();

            try
            {
                while (currentTime < 10)
                {
                    currentTime += 1;
                    OnTimerUpdate?.Invoke(currentTime);

                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        timerCompletionSource?.TrySetResult(false);
                        return;
                    }

                    if(Time.timeScale == 0f)
                    {
                        timerCompletionSource?.TrySetResult(false);
                        return;
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(timerSpeed), false, PlayerLoopTiming.Update, cancellationTokenSource.Token);
                }

                timerCompletionSource?.TrySetResult(true);
            }
            finally
            {
                currentTime = 0;
                OnTimerEnd?.Invoke();
            }
        }

        public override async UniTask<bool> StartCountAsync()
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                return false;
            }

            timerCompletionSource = new UniTaskCompletionSource<bool>();

            try
            {
                await TimerStartCount();
                return await timerCompletionSource.Task;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            finally
            {
                currentTime = 0;
            }
        }
        public override void ResetTimer()
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

    }
}

