using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using System.Linq;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    [CreateAssetMenu(fileName = "New Behaviour Tree", menuName = "GMEngine/XNode/Behaviour Tree")]
    public class GMXBehaviourTree : XNode.NodeGraph, IGMBehaviourTree
    {
        public GMXNode root;
        public ProcessStatus status = ProcessStatus.Running;

        public ProcessStatus Update()
        {
            if (root.status == ProcessStatus.Running)
            {
                status = root.Update();
            }
            return status;
        }

        public IGMBehaviourTree DeepCopy()
        {
            GMXBehaviourTree treeRuntime = Instantiate(this);
            treeRuntime.nodes.Clear();
            //Start the DeepCopy sequence, and the node list's oreder will be the same as hierachy's
            treeRuntime.root = root.DeepCopy(treeRuntime, root) as GMXNode;

            //reorder the native nodes list to coordinate with the copy's
            nodes = SortNodesByHierarchy();


            //Redirect NodePort In Editor
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == null) continue;
                foreach (NodePort port in treeRuntime.nodes[i].Ports)
                {
                    port.Redirect(nodes, treeRuntime.nodes);
                }
            }

            return treeRuntime;
        }

        public List<Node> SortNodesByHierarchy()
        {
            List<Node> sortedNodes = new List<Node>();

            DepthFirstTraversal(root, ref sortedNodes);

            return sortedNodes;

        }
        private void DepthFirstTraversal(Node currentNode, ref List<Node> sortedNodes)
        {
            if (currentNode == null)
            {
                return;
            }

            //sort the branch node
            if (currentNode is IBranchNode branchNode)
            {
                branchNode.Children = branchNode.Children.OrderBy(
                    child => ((GMXNode)child).position.y
                    ).ToList();
            }

            sortedNodes.Add(currentNode);

            // Recursively traverse child nodes
            if (currentNode is IBranchNode)
            {
                foreach (GMXNode child in ((IBranchNode)currentNode).Children)
                {
                    DepthFirstTraversal(child, ref sortedNodes);
                }
            }
            else if (currentNode is ISingletonNode)
            {
                DepthFirstTraversal((GMXNode)((ISingletonNode)currentNode).Child, ref sortedNodes);
            }
        }

        public IGMNode CreateNode(Type type)
        {
            return null;
        }

#if UNITY_EDITOR
        [Header("Editor")]
        public Texture2D initialTexture;
        public Texture2D runningTexture;
        public Texture2D succeedTexture;
        public Texture2D failureTexture;
        public Rect signRect;
#endif
    }
}
