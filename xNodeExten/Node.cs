using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using GMEngine.GMNodes;

namespace GMEngine.GMXNode
{
    public abstract class GMXNode : XNode.Node, IGMNode
    {
        [HideInInspector] public ProcessStatus status = ProcessStatus.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        public string GUID { get => guid; set => guid = value; }

        public ProcessStatus Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            status = OnUpdate();

            if (status != ProcessStatus.Running)
            {
                OnStop();
                started = false;
            }
            return status;
        }

        public virtual IGMNode DeepCopy(IGMBehaviourTree graphHotfix, IGMNode parentRuntime)
        {
            Node.graphHotfix = (XNode.NodeGraph)graphHotfix;
            return Instantiate(this);
        }


        [Obsolete]
        public abstract void RedirectFamily(GMXNode oldNode);

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract ProcessStatus OnUpdate();

        /// <summary>
        /// Called after the NodePort Connection Changed
        /// </summary>
        public abstract void OnConnectionChanged();

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            OnConnectionChanged();
        }
        public override void OnRemoveConnection(NodePort port)
        {
            OnConnectionChanged();
        }

        protected override void Init()
        {
            Debug.Log($"{name} in {graph.name} is Initing");
            started = false;
            status = ProcessStatus.Running;
        }

        public bool TraverseParents(Type type)
        {
            if (type != typeof(IChildNode))
            {
                throw new ArgumentException("Type missmatch occuring, please use IChildNode in hierarchy to traverse");
            }

            if (type == typeof(RootNode))
            {
                return false;
            }
            else
            {
                IChildNode node = (IChildNode)this;

                if (node.Parent is IChildNode parentNode && parentNode.GetType() == type)
                {
                    return true;
                }

                GMXNode parent = (GMXNode)node.Parent;
                return parent.TraverseParents(type);
            }
        }

        public GMNode DeepCopy()
        {
            return null;
        }
    }
}

