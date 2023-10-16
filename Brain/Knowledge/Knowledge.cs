using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine{
    public abstract class Knowledge : ScriptableObject
    {
        public abstract void AddToKnowledge(Transform transform);
        public abstract void RemoveFromKnowledge(Transform transform);

        public abstract bool CheckKnowledge();

        public abstract void ClearKonwledge();

    }
    public interface IKnowledgeData
    {
        public Transform TransformData { get; }
        public bool isNull();
        public void SetNull();
    }
}

