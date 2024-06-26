using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class HandleDragSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DragService _dragService;
        
        private EcsFilter _filter;
        private EcsPool<Destination> _destinationPool;

        public HandleDragSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _filter = world.Filter<Position>().Inc<SelectedTag>().End();
            
            _destinationPool = world.GetPool<Destination>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var destinationComponent = ref _destinationPool.Get(entity);
                
                var pos = _dragService.DragPointToWorldPos();
                destinationComponent.Value = new Vector3(pos.x, destinationComponent.Value.y, pos.y);
            }
        }
    }
}