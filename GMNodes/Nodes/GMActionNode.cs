using GMEngine.GMXNode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public abstract class GMActionNode : GMNode, IChildNode
    {
        private GMNode parent;
        public IGMNode Parent { get => parent; set => parent = (GMNode)value; }
    }
}

