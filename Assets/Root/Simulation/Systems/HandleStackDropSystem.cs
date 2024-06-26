using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class HandleStackDropSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DragService _dragService;
        
        private EcsPool<Destination> _destinationPool;
        private EcsPool<Position> _positionPool;
        private EcsPool<Parent> _parentPool;
        private readonly EventListener _eventListener = new();

        public HandleStackDropSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var filter = world.Filter<Position>().Inc<SelectedTag>().End();
            filter.AddEventListener(_eventListener);
            
            _destinationPool = world.GetPool<Destination>();
            _positionPool = world.GetPool<Position>();
            _parentPool = world.GetPool<Parent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eventListener.OnRemove)
            {
                ref var positionComponent = ref _positionPool.Get(entity);
                var pos = positionComponent.Value.Value;
                ref var destinationComponent = ref _destinationPool.Get(entity);
                
                Vector3 desPos;
                if (_dragService.IsOverCell(pos, out var transform))
                {
                    desPos = transform.position;
                }
                else
                {
                    ref var parentComponent = ref _parentPool.Get(entity);
                    desPos = parentComponent.Value.position;
                }
                
                destinationComponent.Value = new Vector3(desPos.x, 0f, desPos.z);
            }
            _eventListener.OnRemove.Clear();
        }
    }
}