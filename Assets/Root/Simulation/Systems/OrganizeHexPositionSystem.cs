using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class OrganizeHexPositionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<Position> _positionPool;
        private EcsPool<Destination> _destinationPool;
        private EcsPool<Hexes> _hexesPool;
        private EcsPool<Organized> _organizedPool;
        
        private readonly EventListener _eventListener = new();

        private const float HexHeight = 0.15f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var filter = world.Filter<Source>().Inc<Hexes>().Exc<Organized>().End();
            filter.AddEventListener(_eventListener);
            
            _positionPool = world.GetPool<Position>();
            _destinationPool = world.GetPool<Destination>();
            _hexesPool = world.GetPool<Hexes>();
            _organizedPool = world.GetPool<Organized>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                var parentPos = _positionPool.Get(e).Property.Value;
                    
                ref var hexesComponent = ref _hexesPool.Get(e);
                for (var index = 0; index < hexesComponent.Value.Count; index++)
                {
                    var hexEntity = hexesComponent.Value[index];

                    if (!_destinationPool.Has(hexEntity)) _destinationPool.Add(hexEntity);
                    
                    ref var destinationComponent = ref _destinationPool.Get(hexEntity);
                    destinationComponent.Value = new Vector3(parentPos.x, index * HexHeight, parentPos.z);
                }

                _organizedPool.Add(e);
            }
            _eventListener.OnAdd.Clear();
        }
    }
}