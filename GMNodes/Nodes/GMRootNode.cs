using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMRootNode : GMNode, ISingletonNode
    {
        [SerializeField,HideInNodeBody] private GMNode child;

        public IGMNode Child { get => child; set => child = (GMNode)value; }

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override ProcessStatus OnUpdate()
        {
            return child.Update();
        }

        public override GMNode DeepCopy()
        {
            GMRootNode root = Instantiate(this);
            root.child = child.DeepCopy();
            return root;
        }
    }
}

