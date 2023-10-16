using System;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Simple Loop Timer")]
    public class SimpleLoopTimerSO : SimpleTimerSO
    {
        public float loopDelay = 0.5f;

        public override async UniTask<bool> StartCountAsync()
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                return false;
            }

            timerCompletionSource = new UniTaskCompletionSource<bool>();
            
            try
            {
                while (true)
                {
                    await TimerStartCount();
                    await UniTask.Delay(TimeSpan.FromSeconds(loopDelay));
                }
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        public SimpleLoopTimerSO(float loopDelay, float timerSpeed)
        {
            this.loopDelay = loopDelay;
            this.timerSpeed = timerSpeed;
        }
    }
}

