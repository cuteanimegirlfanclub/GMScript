using System.Collections.Generic;
using UnityEngine;
using System;

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
        [HideInNodeBody] public ProcessStatus status = ProcessStatus.Running;
        [HideInNodeBody] public bool started = false;

        [SerializeField, HideInInspector] private string guid;
        public string GUID { get => guid; set => guid = value; }

        public event Action<ProcessStatus, ProcessStatus> StatusChanged;

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

            ProcessStatus previousStatus = status;
            status = OnUpdate();
            if (status != previousStatus)
            {
                OnStatusChanged(previousStatus, status);
            }

            if (status != ProcessStatus.Running)
            {
                OnStop();
                started = false;
            }

            return status;
        }

        protected virtual void OnStatusChanged(ProcessStatus previousStatus, ProcessStatus newStatus)
        {
            StatusChanged?.Invoke(previousStatus, newStatus);
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
