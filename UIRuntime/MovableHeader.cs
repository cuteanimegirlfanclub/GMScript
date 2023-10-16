using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GMEngine
{
    [DisallowMultipleComponent]
    public class MovableHeader : MonoBehaviour, IDragHandler, IBeginDragHandler
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

