using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HandleDragSystem : IEcsInitSystem, IEcsPostRunSystem
    {
        private readonly DragService _dragService;
        
        private EcsFilter _dragFilter;
        private EcsFilter _draggingFilter;
        private EcsPool<Drag> _dragPool;
        private EcsPool<MonoLink<Transform>> _transformPool;

        public HandleDragSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _dragFilter = world.Filter<Drag>().End();
            _draggingFilter = world.Filter<Dragging>().Inc<Hex>().End();

            _dragPool = world.GetPool<Drag>();
            _transformPool = world.GetPool<MonoLink<Transform>>();
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var e in _dragFilter)
            {
                var drag = _dragPool.Get(e);
                foreach (var dragging in _draggingFilter)
                {
                    var transform = _transformPool.Get(dragging).Value;
                    var pos = _dragService.MousePosToWorldPos(drag.MousePosition);
                    transform.position = new Vector3(pos.x, transform.position.y, pos.z);
                }
                
                _dragPool.Del(e);
            }
        }
    }
}