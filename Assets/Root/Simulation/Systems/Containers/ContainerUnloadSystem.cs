using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class ContainerUnloadSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelViewService _levelViewService;
        
        private EcsFilter _unloadRequestFilter;
        private EcsPool<UnloadRequest> _unloadRequestPool;
        private EcsPool<Hexes> _hexesPool;
        private EcsPool<Parent> _parentPool;
        private EcsPool<Source> _sourcePool;
        private EcsPool<Position> _positionPool;

        public ContainerUnloadSystem(LevelViewService levelViewService)
        {
            _levelViewService = levelViewService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _unloadRequestFilter = world.Filter<UnloadRequest>().Inc<Hexes>().End();

            _unloadRequestPool = world.GetPool<UnloadRequest>();
            _hexesPool = world.GetPool<Hexes>();
            _parentPool = world.GetPool<Parent>();
            _sourcePool = world.GetPool<Source>();
            _positionPool = world.GetPool<Position>();    
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _unloadRequestFilter)
            {
                var receiverId = _unloadRequestPool.Get(e).ReceiverId;
                var containerHexes = _hexesPool.Get(e).Value;
                var cellHexes = _hexesPool.Get(receiverId).Value;
                
                cellHexes.AddRange(containerHexes);
                containerHexes.Clear();

                var receiverPos = _positionPool.Get(receiverId).Property.Value;

                foreach (var hex in cellHexes)
                {
                    ref var hexParent = ref _parentPool.Get(hex);
                    hexParent.Property.SetValueAndForceNotify(_levelViewService.HexContainer);
                    var positionProperty = _positionPool.Get(hex).Property;
                    var pos = positionProperty.Value;
                    positionProperty.SetValueAndForceNotify(new Vector3(receiverPos.x, pos.y, receiverPos.z));
                }

                _sourcePool.Del(e);
                _sourcePool.Add(receiverId);
            }
        }
    }
}