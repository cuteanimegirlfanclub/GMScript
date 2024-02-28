using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMSequenceNode : GMCompositeNode
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
            IGMNode child = GetChild(current);
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

            return current == ChildCount() ? ProcessStatus.Success : ProcessStatus.Running;
        }

    }
}

