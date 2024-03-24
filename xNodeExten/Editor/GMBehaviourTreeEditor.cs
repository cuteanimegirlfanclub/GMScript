using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using XNode;
using System;

namespace GMEngine.GMXNode.Editor
{
    [CustomNodeGraphEditor(typeof(GMXBehaviourTree))]
    public class GMBehaviourTreeEditor : NodeGraphEditor
    {
        public Texture signTexture;
        public GMXBehaviourTree graph { get => (GMXBehaviourTree)target;}
        public override void OnCreate()
        {
            GMXBehaviourTree tree = (GMXBehaviourTree)target;

            //Add root node to the tree
            if (tree.root == null)
            {
                XNode.Node.graphHotfix = target;
                tree.root = ScriptableObject.CreateInstance<RootNode>();
                tree.root.graph = target;
                target.nodes.Add(tree.root);
                tree.root.name = "Root";

                AssetDatabase.AddObjectToAsset(tree.root, target);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }

        public override void OnNodeCreated(XNode.Node node)
        {
            base.OnNodeCreated(node);
        }


        public override string GetNodeMenuName(Type type)
        {
            return base.GetNodeMenuName(type);
        }

        public override void RemoveNode(XNode.Node node)
        {
            //avoid user removing the rootnode
            if (node.GetType() == typeof(RootNode)) { return; }
            base.RemoveNode(node);
        }

        public override XNode.Node CopyNode(XNode.Node original)
        {
            //avoid user copying the rootnode
            if (original.GetType() == typeof(RootNode)) { return null; }
            return base.CopyNode(original);
        }
    }
}

