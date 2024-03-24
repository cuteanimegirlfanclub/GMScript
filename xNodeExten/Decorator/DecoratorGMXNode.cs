using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public abstract class DecoratorGMXNode : GMXNode, IChildNode, ISingletonNode
    {
        [Input(connectionType = ConnectionType.Override, backingValue = ShowBackingValue.Never)] public GMXNode parent;
        [Output(connectionType =ConnectionType.Override, backingValue = ShowBackingValue.Never)] public GMXNode child;

        public IGMNode Parent { get => parent; set => parent = (GMXNode)value; }
        public IGMNode Child { get => child; set => child = (GMXNode)value; }

        public override IGMNode DeepCopy(IGMBehaviourTree treeHotfix, IGMNode parentRuntime)
        {
            XNode.Node.graphHotfix = (XNode.NodeGraph)treeHotfix;
            DecoratorGMXNode copy = Instantiate(this);
            copy.graph = (XNode.NodeGraph)treeHotfix;

            copy.parent = (GMXNode)parentRuntime;
            copy.graph.nodes.Add(copy);

            copy.child = (GMXNode)child.DeepCopy(treeHotfix, copy);
            return copy;
        }

        [Obsolete]
        public override void RedirectFamily(GMXNode oldNode)
        {
            DecoratorGMXNode oldTemp = oldNode as DecoratorGMXNode;
            parent = oldTemp.parent;
            child = oldTemp.child;
        }


        public override void OnConnectionChanged()
        {
            //parent part
            NodePort parentPort = GetInputPort("parent");

            if(!parentPort.IsConnected)
            {
                this.parent = null;
            }
            else
            {
                GMXNode parent = parentPort.Connection.node as GMXNode;
                this.parent = parent;
            }


            //children part
            NodePort childPort = GetOutputPort("child"); 

            if (!childPort.IsConnected)
            {
                this.child = null;
            }
            else
            {
                GMXNode child = childPort.Connection.node as GMXNode;
                this.child = child;
            }
        }

        public override object GetValue(NodePort port)
        {
            return this;
        }
    }
}

