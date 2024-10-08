using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class ReturnExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;
        private readonly ViewSettings _viewSettings;

        private EcsWorld _world;
        private EcsFilter _draggingFilter;
        private readonly EventListener _eventListener = new();

        private EcsPool<MoveProcess> _moveProcessPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<WorldPosition> _worldPosPool;
        private EcsPool<MonoLink<Transform>> _transformPool;

        public ReturnExecuteSystem(GameFlowService gameFlowService, ViewSettings viewSettings)
        {
            _gameFlowService = gameFlowService;
            _viewSettings = viewSettings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _draggingFilter = _world.Filter<Dragging>().End();
            _draggingFilter.AddEventListener(_eventListener);

            _hexPool = _world.GetPool<Hex>();
            _moveProcessPool = _world.GetPool<MoveProcess>();
            _worldPosPool = _world.GetPool<WorldPosition>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnRemove)
            {
                var hex = _hexPool.Get(e);
                
                if (_worldPosPool.Has(hex.Target.Id))
                {
                    hex.Target.Unpack(_world, out var target);
                    
                    var transform = _transformPool.Get(e).Value;
                    
                    var targetPos = _worldPosPool.Get(target).Value;
                    
                    ref var process = ref _gameFlowService.StartNewProcess(_moveProcessPool, e);
                    process.Offset = new Vector3(targetPos.x - transform.position.x, 0, targetPos.z - transform.position.z);
                    process.Speed = _viewSettings.HexHorizontalSpeed;
                }
            }
            _eventListener.OnRemove.Clear();
        }
    }
}