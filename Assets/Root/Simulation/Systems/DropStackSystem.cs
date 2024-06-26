using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class DropStackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<Destination> _destinationPool;
        private readonly EventListener _eventListener = new();
        
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var filter = world.Filter<Position>().Inc<SelectedTag>().End();
            filter.AddEventListener(_eventListener);
            
            _destinationPool = world.GetPool<Destination>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _eventListener.OnRemove)
            {
                ref var destinationComponent = ref _destinationPool.Get(entity);
                var desPos = destinationComponent.Value;
                destinationComponent.Value = new Vector3(desPos.x, 0f, desPos.z);
            }
            _eventListener.OnRemove.Clear();
        }
    }
}