using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class DropExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ProcessService _processService;
        private readonly CoreViewSettings _coreViewSettings;

        private EcsWorld _world;
        private EcsFilter _draggingFilter;
        private readonly EventListener _eventListener = new();
        private EcsPool<MoveProcess> _movePool;
        private EcsPool<HeightOffset> _offsetPool;
        private EcsPool<Hex> _hexPool;

        public DropExecuteSystem(ProcessService processService, CoreViewSettings coreViewSettings)
        {
            _processService = processService;
            _coreViewSettings = coreViewSettings;
        }


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _draggingFilter = _world.Filter<Dragging>().End();
            _draggingFilter.AddEventListener(_eventListener);
            _movePool = _world.GetPool<MoveProcess>();
            _offsetPool = _world.GetPool<HeightOffset>();
            _hexPool = _world.GetPool<Hex>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnRemove)
            {
                var hex = _hexPool.Get(e);
                hex.Target.Unpack(_world, out var target);
                
                var offset = 0f;
                if (_offsetPool.Has(target))
                {
                    offset = _offsetPool.Get(target).Value;
                }
                
                ref var process = ref _processService.StartNewProcess(_movePool, e);
                process.Offset = Vector3.down * (_coreViewSettings.HexFlightHeight - offset);
                process.Speed = _coreViewSettings.HexVerticalSpeed;
            }
            _eventListener.OnRemove.Clear();
        }
    }
}