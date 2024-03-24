using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class TreeUpdateNode : GMActionNode
    {
        [SerializeField] private GMBehaviourTree tree;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override ProcessStatus OnUpdate()
        {
            tree.Update();
            return ProcessStatus.Success;
        }

        public override GMNode DeepCopy()
        {
            TreeUpdateNode node = Instantiate(this);
            node.tree = (GMBehaviourTree)tree.DeepCopy();
            return node;
        }
    }
}

