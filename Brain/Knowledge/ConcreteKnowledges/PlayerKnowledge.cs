using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Game
{
    public class PlayerKnowledge : MonoBehaviour, IKnowledge
    {
        //items in player's sight
        [SerializeField]
        private List<Transform> knowledges = new List<Transform>();

        //represent the ground item that player could pick
        [SerializeField] private InventoryItem grabbleItem;
        public InventoryItem GrabbleItem { get => grabbleItem; }

        //the Inventory UI selected item
        [SerializeField] private InventoryItem selectingItem;
        public InventoryItem SelectingItem { get => selectingItem; }

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
        public void SetGrabbleItem(InventoryItem item)
        {
            grabbleItem = item;
        }

        public void SetSelectingItem(InventoryItem item)
        {
            selectingItem = item;
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

