using GMEngine.GMAddressables;
using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMEngine.GMNodes
{
    public class UnityEventInvoker : GMActionNode
    {
        [HideInNodeBody] public UnityEvent _event;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
            
        }

        protected override ProcessStatus OnUpdate()
        {
            return ProcessStatus.Success;
        }
    }
}

