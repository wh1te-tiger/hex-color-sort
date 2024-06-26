using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class DragService 
    {
        #region Dependencies
        
        private readonly Camera _camera;
        
        #endregion
        
        private const float ClipPlaneDistance = -1f;

        public DragService(Camera camera)
        {
            _camera = camera;
        }
        
        public void HandleBeginDragEvent(PointerEventData pointerData)
        {
            var screenPos = new Vector3(pointerData.position.x, pointerData.position.y, _camera.nearClipPlane);
            var ray = _camera.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out var hit, 20, LayerMask.GetMask("Selectable"))) return;
                
            var selectable = hit.transform.gameObject.GetComponent<ISelectable>();
            selectable.IsSelected = true;
        }
        
        public void HandleEndDragEvent(PointerEventData pointerData)
        {
            var screenPos = new Vector3(pointerData.position.x, pointerData.position.y, _camera.nearClipPlane);
            var ray = _camera.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out var hit, 20, LayerMask.GetMask("Selectable"))) return;

            var stackTransform = hit.transform;
            
            var selectable = stackTransform.gameObject.GetComponent<ISelectable>();
            selectable.IsSelected = false;
        }
        
        public Vector2 DragPointToWorldPos()
        {
            var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            var worldPoint = _camera.ScreenToWorldPoint(new Vector3(position.x, position.y, _camera.nearClipPlane));
            var plane = new Plane(Vector3.up, ClipPlaneDistance);
            var ray = _camera.ScreenPointToRay(position);
            if (plane.Raycast(ray, out var enter))
            {
                worldPoint = ray.GetPoint(enter);
            }
            return new Vector2(worldPoint.x, worldPoint.z);
        }

        public bool IsOverCell(Vector3 pos, out Transform cell)
        {
            cell = null;
            var screenPos = _camera.WorldToScreenPoint(pos);
            var ray = _camera.ScreenPointToRay(screenPos);
            
            if (!Physics.Raycast(ray, out var hit, 20, LayerMask.GetMask("Cell"))) return false;
            
            cell = hit.transform;
            return true;
        }
    }
}