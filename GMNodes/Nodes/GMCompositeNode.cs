using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public abstract class GMCompositeNode : GMNode, IChildNode, IBranchNode, IEnumerable<IGMNode>
    {
        private GMNode parent;

        [SerializeField,HideInNodeBody]
        private List<GMNode> children = new List<GMNode>();

        public IGMNode Parent { get => parent; set => parent = (GMNode)value; }
        public IEnumerable<IGMNode> Children
        {
            get => children;
            set
            {
                if (value is List<GMNode> list)
                { 
                    children = list;
                }
                else if (value is List<IGMNode>)
                {
                    children = (List<GMNode>)value;
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
                children.IndexOf((GMNode)node);
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

        public void AddChild(GMNode node)
        {
            children.Add(node);
        }

        public void RemoveChild(GMNode node)
        {
            children.Remove(node);
        }

        public int ChildCount()
        {
            return children.Count;
        }

        public List<GMNode> GetChildrenList()
        {
            return children;
        }

        public override GMNode DeepCopy()
        {
            GMCompositeNode root = Instantiate(this);
            root.children = children.ConvertAll(c => c.DeepCopy());
            return root;
        }

        IEnumerator<IGMNode> IEnumerable<IGMNode>.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }
    }

}
