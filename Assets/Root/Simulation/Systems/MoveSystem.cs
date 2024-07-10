using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class MoveSystem : IEcsInitSystem, IEcsFixedRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<Position> _positionPool;
        private EcsPool<Destination> _destinationPool;

        private const float MinDistance = .01f;
        private const float HorizontalSpeed = 45f;
        private const float VerticalSpeed = 3f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<Position>().Inc<Destination>().Exc<Delay>().End();
            _positionPool = world.GetPool<Position>();
            _destinationPool = world.GetPool<Destination>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var destination = ref _destinationPool.Get(entity);
                if (destination.IsCompleted)
                {
                    continue;
                }
                var targetPos = destination.Value;
                
                ref var pos = ref _positionPool.Get(entity);
                var currentPos = pos.Property.Value;
                
                var difference = targetPos - currentPos;
                if (difference.magnitude < MinDistance)
                {
                    destination.IsCompleted = true;
                    continue;
                }
                
                var direction = difference.normalized;
                var deltaPositionXZ = direction * HorizontalSpeed * Time.deltaTime;
                var deltaPositionY = direction * VerticalSpeed * Time.deltaTime;
                var deltaPos = new Vector3(deltaPositionXZ.x, deltaPositionY.y, deltaPositionXZ.z);
                pos.Property.SetValueAndForceNotify(Vector3.MoveTowards(currentPos, targetPos, deltaPos.magnitude));
            }
        }
    }
}