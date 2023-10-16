using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Knowledge/Enemy Knowledge")]
    public class EnemyKnowledge : Knowledge
    {
        public List<Transform> knowledges;

        public override void AddToKnowledge(Transform transform)
        {
            if (knowledges.Contains(transform)) return;
            knowledges.Add(transform);
        }

        public override void RemoveFromKnowledge(Transform transform)
        {
            if (knowledges.Contains(transform)) knowledges.Remove(transform);
            else return;
        }

        public override void ClearKonwledge()
        {
            knowledges.Clear();
        }

        public override bool CheckKnowledge()
        {
            return knowledges.Count > 0;
        }
    }

}

