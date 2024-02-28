using GMEngine.GMXNode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public abstract class GMDecoratorNode : GMNode, IChildNode, ISingletonNode
    {
        private GMNode parent;
        [SerializeField, HideInNodeBody] private GMNode child;

        public IGMNode Parent { get => parent; set => parent = (GMNode)value; }
        public IGMNode Child { get => child; set => child = (GMNode)value; }

        public override GMNode DeepCopy()
        {
            GMDecoratorNode root = Instantiate(this);
            root.child = child.DeepCopy();
            return root;
        }
    }
}

