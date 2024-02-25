using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public class SequenceGMXNode : CompositeGMXNode
    {
        protected int current;
        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
        }

        protected override ProcessStatus OnUpdate()
        {
            GMXNode child = children[current];
            switch (child.Update())
            {
                case ProcessStatus.Running:
                    return ProcessStatus.Running;
                case ProcessStatus.Failure:
                    return ProcessStatus.Failure;
                case ProcessStatus.Success:
                    current++;
                    break;
            }

            return current == children.Count ? ProcessStatus.Success : ProcessStatus.Running;
        }

    }
}

