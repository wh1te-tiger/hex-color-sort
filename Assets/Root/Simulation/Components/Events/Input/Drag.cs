using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public struct BeginDragEvent
    {
        public GameObject Sender;
        public PointerEventData PointerEventData;
    }
    
    public struct EndDragEvent
    {
        public GameObject Sender;
        public PointerEventData PointerEventData;
    }
}