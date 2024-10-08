using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class RiseExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;
        private readonly ViewSettings _viewSettings;

        private EcsWorld _world;
        private EcsFilter _draggingFilter;
        private readonly EventListener _eventListener = new();
        private EcsPool<MoveProcess> _movePool;

        public RiseExecuteSystem(GameFlowService gameFlowService, ViewSettings viewSettings)
        {
            _gameFlowService = gameFlowService;
            _viewSettings = viewSettings;
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
                ref var process = ref _gameFlowService.StartNewProcess(_movePool, e);
                process.Offset = Vector3.up * _viewSettings.HexFlightHeight;
                process.Speed = _viewSettings.HexVerticalSpeed;
            }
            _eventListener.OnAdd.Clear();
        }
    }
}