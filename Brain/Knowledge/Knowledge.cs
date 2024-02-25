using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine{
    public interface IKnowledge
    {
        public void AddToKnowledge(Transform transform);
        public void RemoveFromKnowledge(Transform transform);

        public bool CheckKnowledge();

        public void ClearKonwledge();

    }
}

