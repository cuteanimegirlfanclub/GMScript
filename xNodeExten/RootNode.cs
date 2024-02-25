using System;
using UnityEngine;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode {
    [CreateNodeMenu("",menuName ="")]
    public class RootNode : GMXNode, ISingletonNode
    {
        [Output(connectionType = ConnectionType.Override)] public GMXNode child;

        public IGMNode Child { get => child; set => child = (GMXNode)value; }

        public override void OnConnectionChanged()
        {
            //children part
            NodePort childPort = GetOutputPort("child");

            if (!childPort.IsConnected)
            {
                this.child = null;
                Debug.LogWarning($"{graph.name}'s RootNode Require A Child In order to Execute!");
            }
            else
            {
                GMXNode child = childPort.Connection.node as GMXNode;
                this.child = child;
            }
        }

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

        public override IGMNode DeepCopy(IGMBehaviourTree treeHotfix, IGMNode parentRuntime)
        {
            XNode.Node.graphHotfix = (XNode.NodeGraph)treeHotfix;
            RootNode root = Instantiate(this);
            root.graph = (XNode.NodeGraph)treeHotfix;

            ((XNode.NodeGraph)treeHotfix).nodes.Add(root);
            root.child = (GMXNode)child.DeepCopy(treeHotfix, root);
            return root;
        }

        [Obsolete]
        public override void RedirectFamily(GMXNode oldNode)
        {
            RootNode rootTemp = oldNode as RootNode;
            child = rootTemp.child;
        }

        public override object GetValue(NodePort port)
        {
            return this;
        }
    }
}

