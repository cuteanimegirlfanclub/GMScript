using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public abstract class ActionGMXNode : GMXNode, IChildNode
    {
        [Input(connectionType = ConnectionType.Override, backingValue = ShowBackingValue.Never)] public GMXNode parent;

        public IGMNode Parent { get => parent; set => parent = (GMXNode)value; }

        public override void OnConnectionChanged()
        {
            NodePort parentPort = GetInputPort("parent");
            if (!parentPort.IsConnected)
            {
                this.parent = null;
            }
            else
            {
                GMXNode parent = parentPort.Connection.node as GMXNode;
                this.parent = parent;
            }
        }

        public override IGMNode DeepCopy(IGMBehaviourTree graphHotfix, IGMNode parentRuntime)
        {
            XNode.Node.graphHotfix = (XNode.NodeGraph)graphHotfix;
            ActionGMXNode copy = Instantiate(this);
            copy.parent = (GMXNode)parentRuntime;
            copy.graph.nodes.Add(copy);
            return copy;
        }

        [Obsolete]
        public override void RedirectFamily(GMXNode oldNode)
        {
            ActionGMXNode oldTemp = oldNode as ActionGMXNode;
            parent = oldTemp.parent;
        }
    }
}

