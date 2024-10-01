using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    [RequireComponent(typeof(EntityProvider))]
    public class InputReceiver : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private EntityProvider _provider;
        private void Awake()
        {
            _provider = GetComponent<EntityProvider>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_provider.Has<Empty>())
            {
                //Debug.Log("DOWN");
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_provider.Has<Empty>())
            {
                //Debug.Log("UP");
            }
            
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("Dragged");
        }
    }
}