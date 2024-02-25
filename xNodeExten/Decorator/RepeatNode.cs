using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public class RepeatNode : DecoratorGMXNode
    {
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override ProcessStatus OnUpdate()
        {
            child.Update();
            return ProcessStatus.Running;
        }

    }
}

