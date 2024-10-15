using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class RiseExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ProcessService _processService;
        private readonly CoreViewSettings _coreViewSettings;

        private EcsWorld _world;
        private EcsFilter _draggingFilter;
        private readonly EventListener _eventListener = new();
        private EcsPool<MoveProcess> _movePool;

        public RiseExecuteSystem(ProcessService processService, CoreViewSettings coreViewSettings)
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
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                ref var process = ref _processService.StartNewProcess(_movePool, e);
                process.Offset = Vector3.up * _coreViewSettings.HexFlightHeight;
                process.Speed = _coreViewSettings.HexVerticalSpeed;
            }
            _eventListener.OnAdd.Clear();
        }
    }
}