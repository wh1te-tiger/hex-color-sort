using UnityEngine;

namespace Scripts
{
    public class DragService
    {
        private readonly ViewSettings _viewSettings;
        
        #region Dependencies
        
        private readonly Camera _camera = Camera.main;

        public DragService(ViewSettings viewSettings)
        {
            _viewSettings = viewSettings;
        }

        #endregion

        public bool CheckSlotTarget(Vector2 mousePos, out int entity)
        {
            entity = -1;
            var ray = _camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, 100))
            {
                var provider = hit.collider.GetComponent<EntityProvider>();
                if (provider == null) return false;

                if (!provider.TryGetEntity(out entity)) return false;

                if (provider.Has<Slot>() && !provider.Has<Empty>()) return true;
            }
            
            return false;
        }

        public Coordinates ScreenPosToFieldCoordinates(Vector2 mousePos)
        {
            var worldPos = MousePosToWorldPos(mousePos);
            return FieldService.GetHexCoordinates(new Vector3(worldPos.x, 0, worldPos.z), _viewSettings.CellWidth);
        }
        
        public Vector3 MousePosToWorldPos(Vector2 mousePos)
        {
            var ray = _camera.ScreenPointToRay(mousePos);
            var plane = new Plane(Vector3.up, -_viewSettings.CellHeight);
            return plane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : new Vector3();
        }
    }
}