using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public class DebugMessageXNode : ActionGMXNode
    {
        public string message;

        protected override void OnStart()
        {
            Debug.Log($"{name} in {graph.name} Started {message}");
        }

        protected override void OnStop()
        {
            Debug.Log($"{name} in {graph.name} Stopped {message}");
        }

        protected override ProcessStatus OnUpdate()
        {
            Debug.Log($"{name} in {graph.name} Updating {message}");
            return ProcessStatus.Success;
        }
    }
}

