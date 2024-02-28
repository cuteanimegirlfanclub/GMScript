using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine.GMNodes
{
    public class GMWaitNode : GMActionNode
    {
        public float duration = 1;
        private float startTime;
        protected override void OnStart()
        {
            startTime = Time.time;
        }

        protected override void OnStop()
        {
        }

        protected override ProcessStatus OnUpdate()
        {
            if (Time.time - startTime > duration)
            {
                return ProcessStatus.Success;
            }
            return ProcessStatus.Running;
        }
    }

}
