using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class PlayerKnowledge : MonoBehaviour, IKnowledge
    {
        //items in player's sight
        [SerializeField]
        private List<Transform> knowledges = new List<Transform>();

        //represent the ground item that player could pick
        public GameObject grabbleItem;
        //the Inventory UI selected item
        public GameObject selectingItemGO;

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

        //logic need to be improved
        public void SetGrabbleItem(GameObject gameObject)
        {
            grabbleItem = gameObject;
            Debug.Log("GrabbleItem Setted");
        }

        public void SetSelectingItem(GameObject gameObject)
        {
            selectingItemGO = gameObject;
        }

        public GameObject GetSelectingItem()
        {
            GameObject selected = selectingItemGO;
            Debug.Log($"Geting Selecting Item of {selected.name}");
            return selected;
        }

        public bool CheckKnowledge()
        {
            return knowledges.Count > 0;
        }

        public bool haveGrabbleItem()
        {
            return grabbleItem != null;
        }
    }
}

