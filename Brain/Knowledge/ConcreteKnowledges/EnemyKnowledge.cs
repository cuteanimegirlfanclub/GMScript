using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class EnemyKnowledge : MonoBehaviour, IKnowledge
    {
        public List<Transform> knowledges;

        public void AddToKnowledge(Transform transform)
        {
            if (knowledges.Contains(transform)) return;
            knowledges.Add(transform);
        }

        public void RemoveFromKnowledge(Transform transform)
        {
            if (knowledges.Contains(transform)) knowledges.Remove(transform);
            else return;
        }

        public void ClearKonwledge()
        {
            knowledges.Clear();
        }

        public bool CheckKnowledge()
        {
            return knowledges.Count > 0;
        }
    }

}

