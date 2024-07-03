using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class OrganizeHexPositionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _initRequestFilter;
        private EcsPool<Position> _positionPool;
        private EcsPool<Hexes> _hexesPool;
        private EcsPool<ChildRoot> _childRootPool;
        private EcsPool<Parent> _parentPool;

        private const float HexHeight = 0.15f;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().Inc<Hexes>().Inc<ChildRoot>().End();
            
            _positionPool = world.GetPool<Position>();
            _parentPool = world.GetPool<Parent>();
            _hexesPool = world.GetPool<Hexes>();
            _childRootPool = world.GetPool<ChildRoot>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _initRequestFilter)
            {
                ref var childRootComponent = ref _childRootPool.Get(e);
                ref var hexesComponent = ref _hexesPool.Get(e);
                for (var index = 0; index < hexesComponent.Value.Count; index++)
                {
                    var hexEntity = hexesComponent.Value[index];
                    
                    ref var parentComponent = ref _parentPool.Get(hexEntity);
                    parentComponent.Property.Value = childRootComponent.Value;

                    var parentPos = parentComponent.Property.Value.position;
                    ref var positionComponent = ref _positionPool.Get(hexEntity);
                    positionComponent.Property.Value = new Vector3(parentPos.x, index * HexHeight, parentPos.z);
                }
            }
        }
    }
}