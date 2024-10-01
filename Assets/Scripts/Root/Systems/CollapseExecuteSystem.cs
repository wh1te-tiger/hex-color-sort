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
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _collapseFilter)
            {
                var p = _world.NewEntity();
                ref var process = ref _gameFlowService.StartNewProcess(_processPool, p);
                process.Target = _world.PackEntity(e);
            }
        }
    }
}