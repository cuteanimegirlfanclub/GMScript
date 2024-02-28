using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMRepeatNode : GMDecoratorNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {

        }

        protected override ProcessStatus OnUpdate()
        {
            Child.Update();
            return ProcessStatus.Running;
        }
    }

}
