using UnityEngine;

namespace Scripts
{
    public class DragService
    {
        #region Dependencies
        
        private readonly Camera _camera = Camera.main;
        
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
        
        public bool CheckCellTarget(Vector2 mousePos, out int entity)
        {
            entity = -1;
            var ray = _camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Cell") ))
            {
                var provider = hit.collider.GetComponent<EntityProvider>();
                if (provider == null) return false;

                if (!provider.TryGetEntity(out entity)) return false;

                if (provider.Has<Cell>() && provider.Has<Empty>()) return true;
            }
            
            return false;
        }

        public Vector3 MousePosToWorldPos(Vector2 mousePos)
        {
            var ray = _camera.ScreenPointToRay(mousePos);
            var plane = new Plane(Vector3.up, 0);
            return plane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : new Vector3();
        }
    }
}