using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GMEngine.UI
{
    [DisallowMultipleComponent]
    public class MovableUI : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        private Vector2 offset;
        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = eventData.position - (Vector2)transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.parent.position = eventData.position - offset;
        }
    }
}

