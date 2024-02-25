using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public abstract class CompositeGMXNode : GMXNode, IChildNode, IBranchNode
    {
        [Input(connectionType = ConnectionType.Override, backingValue = ShowBackingValue.Never)] public GMXNode parent;
        [Output(backingValue = ShowBackingValue.Never)] public List<GMXNode> children = new List<GMXNode>();

        public IGMNode Parent { get => parent; set => parent = (GMXNode)value; }
        public IEnumerable<IGMNode> Children
        {
            get => children;
            set
            {
                if (value is List<GMXNode> list)
                {
                    children = list;
                }else if(value is List<IGMNode>)
                {
                    children = (List<GMXNode>)value;
                }
                else 
                {
                    Debug.LogError("Value must be of type of List<GMNode>");
                }
            }
        }

        public int GetIndex(IGMNode node)
        {
            if (Children != null)
            {
                children.IndexOf((GMXNode)node);
            }
            return -1;
        }

        public IGMNode GetChild(int i)
        {
            if (Children != null && i >= 0)
            {
                return children[i];
            }
            return null;
        }

        public int ChildrenCount()
        {
            return children.Count;
        }

        public override IGMNode DeepCopy(IGMBehaviourTree treeHotfix, IGMNode parentRuntime)
        {
            Node.graphHotfix = (XNode.NodeGraph)treeHotfix;
            CompositeGMXNode copy = Instantiate(this);
            copy.graph = (XNode.NodeGraph)treeHotfix;

            copy.parent = (GMXNode)parentRuntime; 
            copy.graph.nodes.Add(copy);
            copy.Children = children.ConvertAll(c => c.DeepCopy(treeHotfix, copy));
            return copy;
        }

        [Obsolete]
        public override void RedirectFamily(GMXNode oldNode)
        {
            CompositeGMXNode node = oldNode as CompositeGMXNode;
            parent = node.parent;
            children = node.children;
        }

        public override void OnConnectionChanged()
        {
            //parent part
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

            //children part
            NodePort childrenPort = GetOutputPort("children");
            children.Clear();
            if(!childrenPort.IsConnected)
            {
                return;
            }
            else
            {
                for(int i = 0;  i < childrenPort.ConnectionCount; i++)
                {
                    GMXNode child = childrenPort.GetConnection(i).node as GMXNode;
                    children.Add(child);
                }
            }

        }

        public override object GetValue(NodePort port)
        {
            return children;
        }
    }
}

