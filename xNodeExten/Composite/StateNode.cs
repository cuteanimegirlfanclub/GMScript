using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public class StateNode : SequenceGMXNode
    {
        protected override void OnStart()
        {
            //start the on start action sequence
            //code

            //set the current node and start the on update sequence
        }

        protected override void OnStop()
        {
            //start the on stop action sequence
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

        public override void OnConnectionChanged()
        {
            base.OnConnectionChanged();

            //
        }

        protected override void Init()
        {
            base.Init();
        }
    }
}

