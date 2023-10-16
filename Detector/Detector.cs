using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public abstract class Detector
    {
        public Knowledge knowledge;
        public int scanFrequency;
        public LayerMask layers;
        public LayerMask occulusionLayers;

        public SimpleLoopTimerSO timer;

        protected abstract Mesh CreateDetectMesh();

        protected abstract void Scan();
        protected abstract bool IsInSight(GameObject obj);

        protected async void RunTimer()
        {
            bool timerCompleted = await timer.StartCountAsync();
            if (timerCompleted) { Scan(); }
        }

    }

}

