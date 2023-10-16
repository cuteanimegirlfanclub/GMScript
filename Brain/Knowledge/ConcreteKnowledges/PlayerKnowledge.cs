using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Knowledge/PlayerKnowledge")]
    public class PlayerKnowledge : Knowledge
    {
        //items in player's sight
        public List<Transform> knowledges;

        //represent the ground item that player could pick
        public GameObject grabbleItem;
        //the Inventory UI selected item
        private GameObject selectingItemGO;

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

        //logic need to be improved
        public void SetGrabbleItem(GameObject gameObject)
        {
            grabbleItem = gameObject;
        }

        public void SetSelectingItem(GameObject gameObject)
        {
            selectingItemGO = gameObject;
        }

        /// <summary>
        /// the selecting item will be null after you get it
        /// </summary>
        /// <returns></returns>
        public GameObject GetSelectingItem()
        {
            GameObject selected = selectingItemGO;
            //selectingItemGO = null;
            return selected;
        }

        public override bool CheckKnowledge()
        {
            return knowledges.Count > 0;
        }

        public bool haveGrabbleItem()
        {
            return grabbleItem != null;
        }
    }
}

