using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public interface IGMBehaviourTree
    {
        IGMBehaviourTree DeepCopy();
        ProcessStatus Update();
    }

    [CreateAssetMenu(menuName = "GMEngine/Node Graph/Behaviour Tree")]
    public class GMBehaviourTree : ScriptableObject, IGMBehaviourTree
    {
        [SerializeField]
        private GMNode root;
        public ProcessStatus status = ProcessStatus.Running;

        public GMNode RootNode { get { return root; } set { root = value; } }

        private GMNode current;
        public GMNode Current => current;
   
        public IGMBehaviourTree DeepCopy()
        {
            GMBehaviourTree tree = Instantiate(this);
            tree.root = tree.root.DeepCopy();
#if UNITY_EDITOR
            tree.nodes = new List<GMNode>();
            Traverse(tree.root, (n) =>
            {
                tree.nodes.Add(n);
            });
#endif
            return tree;
        }

        public ProcessStatus Update()
        {
            if (root.status == ProcessStatus.Running)
            {
                status = root.Update();
            }
            return status;
        }
#if UNITY_EDITOR
        /// <summary>
        /// nodes cache for unity editor
        /// </summary>
        public List<GMNode> nodes = new List<GMNode>();

        public void Traverse(GMNode node, Action<GMNode> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach(child => { Traverse(child, visiter); });
            }
        }

        public GMNode CreateNode(System.Type type)
        {
            GMNode node = ScriptableObject.CreateInstance(type) as GMNode;
            node.name = type.Name;
            node.GUID = GUID.Generate().ToString();
            Undo.RecordObject(this, "Node Graph(Create Node)");
            nodes.Add(node);

            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Node Graph(Create Node)");

            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(GMNode node)
        {
            Undo.RecordObject(this, "Node Graph(Remove Node)");
            Debug.Log($"Deleting {node.name} in tree{this.name}");
            nodes.Remove(node);

            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(GMNode parent, GMNode child)
        {
            GMDecoratorNode decorator = parent as GMDecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Node Graph(Add Child)");
                decorator.Child = child;
                EditorUtility.SetDirty(decorator);
            }

            GMRootNode root = parent as GMRootNode;
            if (root)
            {
                Undo.RecordObject(root, "Node Graph(Add Child)");
                root.Child = child;
                EditorUtility.SetDirty(root);
            }

            GMCompositeNode composite = parent as GMCompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Node Graph(Add Child)");
                composite.AddChild(child);
                EditorUtility.SetDirty(composite);
            }
        }

        public void RemoveChild(GMNode parent, GMNode child)
        {
            GMDecoratorNode decorator = parent as GMDecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Node Graph(Remove Child)");
                decorator.Child = null;
                EditorUtility.SetDirty(decorator);
            }

            GMRootNode root = parent as GMRootNode;
            if (root)
            {
                Undo.RecordObject(root, "Node Graph(Remove Child)");
                root.Child = null;
                EditorUtility.SetDirty(root);
            }

            GMCompositeNode composite = parent as GMCompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Node Graph(Remove Child)");
                composite.RemoveChild(child);
                EditorUtility.SetDirty(composite);
            }
        }

        public List<GMNode> GetChildren(GMNode parent)
        {
            List<GMNode> children = new List<GMNode>();
            GMDecoratorNode decorator = parent as GMDecoratorNode;
            if (decorator && decorator.Child != null)
            {
                children.Add((GMNode)decorator.Child);
            }

            GMRootNode root = parent as GMRootNode;
            if (root && root.Child != null)
            {
                children.Add((GMNode)root.Child);
            }

            GMCompositeNode composite = parent as GMCompositeNode;
            if (composite)
            {
                return (List<GMNode>)composite.Children;
            }
            return children;
        }

#endif
    }
}