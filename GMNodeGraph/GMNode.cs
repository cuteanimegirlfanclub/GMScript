using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using XNodeEditor;

namespace GMEngine.GMNodes
{
    public enum ProcessStatus
    {
        Running,
        Failure,
        Success
    }

    public interface IGMNode
    {
        GMNode DeepCopy();
        ProcessStatus Update();

        public string GUID { get; set; }
    }

    public interface ISingletonNode
    {
        public IGMNode Child { get; set; }
    }

    public interface IBranchNode
    {
        public IEnumerable<IGMNode> Children { get; set; }

        public int GetIndex(IGMNode node);
        public IGMNode GetChild(int i);
    }

    public interface IChildNode
    {
        public IGMNode Parent { get; set; }
    }

    public abstract class GMNode : ScriptableObject, IGMNode
    {
        [HideInInspector] public ProcessStatus status = ProcessStatus.Running;
        [HideInInspector] public bool started = false;

        [SerializeField, HideInInspector] private string guid;
        public string GUID { get => guid; set => guid = value; }

        public virtual GMNode DeepCopy()
        {
            return Instantiate(this);
        }

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

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract ProcessStatus OnUpdate();

#if UNITY_EDITOR
        [HideInInspector]public string description;
        [HideInInspector]public Vector2 uiPosition;
#endif
    }
}
