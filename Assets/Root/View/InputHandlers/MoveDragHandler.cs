using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class MoveDragHandler : MonoBehaviour, IMoveDragHandler
    {
        public InputData InputData { get; private set; } 
        public event Action<IDragHandler> DragStarted;
        public event Action<IDragHandler> DragEnded;

        private bool IsEmpty => transform.childCount == 0;
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if ( IsEmpty ) return;
            
            DragStarted?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if ( IsEmpty ) return;
            
            var target = transform.GetChild(0);
            InputData = new InputData(target, eventData.position, eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if ( IsEmpty ) return;
            
            DragEnded?.Invoke(this);
        }
    }
}