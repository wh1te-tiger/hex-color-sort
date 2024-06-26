using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class MoveDragHandler : MonoBehaviour, IMoveDragHandler
    {
        #region Callbacks

        public void OnBeginDrag(PointerEventData eventData)
        {
            EcsUnityEvents.RegisterBeginDragEvent(gameObject, eventData);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            EcsUnityEvents.RegisterEndDragEvent(gameObject, eventData);
        }

        #endregion
    }
}