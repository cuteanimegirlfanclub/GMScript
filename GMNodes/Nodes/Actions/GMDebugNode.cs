using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMDebugNode : GMActionNode
    {
        [TextArea(2,1)]public string message;
        public bool log = false;
        protected override void OnStart()
        {
            if (!log) return;
            Debug.Log($"{name}{message}OnStart");
        }

        protected override void OnStop()
        {
            if (!log) return;
            Debug.Log($"{name}{message}OnStop");
        }

        protected override ProcessStatus OnUpdate()
        {
            if (log)
            {
                Debug.Log($"{name}{message}OnStop");
            }
            return ProcessStatus.Success;
        }
    }

}
