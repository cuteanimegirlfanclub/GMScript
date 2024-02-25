using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public class WaitXNode : ActionGMXNode
    {
        [Range(0, 10)]
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

