using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMDebugNode : GMActionNode
    {
        [TextArea(2,1)]public string message;
        protected override void OnStart()
        {
            Debug.Log($"{name}{message}OnStart");
        }

        protected override void OnStop()
        {
            Debug.Log($"{name}{message}OnStop");
        }

        protected override ProcessStatus OnUpdate()
        {
            Debug.Log($"{name}{message}OnStop");
            return ProcessStatus.Success;
        }
    }

}
