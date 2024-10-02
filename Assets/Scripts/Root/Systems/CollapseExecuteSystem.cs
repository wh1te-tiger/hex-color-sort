using Leopotam.EcsLite;

namespace Scripts
{
    public class CollapseExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;

        private EcsWorld _world;
        private EcsFilter _collapseFilter;
        private EcsPool<CollapseRequest> _collapsePool;
        private EcsPool<CollapseProcess> _processPool;
        private EcsPool<TargetChanged> _targetChangedPool;
        private EcsPool<Hex> _hexPool;

        public CollapseExecuteSystem(GameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _collapseFilter = _world.Filter<CollapseRequest>().End();
            _collapsePool = _world.GetPool<CollapseRequest>();
            _processPool = _world.GetPool<CollapseProcess>();
            _targetChangedPool = _world.GetPool<TargetChanged>();
            _hexPool = _world.GetPool<Hex>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _collapseFilter)
            {
                ref var hex = ref _hexPool.Get(e);
                ref var targetChanged = ref _targetChangedPool.Send();
                targetChanged.Old = hex.Target;
                hex.Target = default;
                
                ref var process = ref _gameFlowService.StartNewProcess(_processPool, e);
                process.Target = _world.PackEntity(e);
                
                _collapsePool.Del(e);
            }
        }
    }
}