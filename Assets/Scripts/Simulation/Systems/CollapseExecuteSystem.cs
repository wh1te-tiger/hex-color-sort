using Leopotam.EcsLite;

namespace Scripts
{
    public class CollapseExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ProcessService _processService;

        private EcsWorld _world;
        private EcsFilter _collapseFilter;
        private EcsPool<CollapseRequest> _collapsePool;
        private EcsPool<CollapseProcess> _processPool;
        private EcsPool<TargetChanged> _targetChangedPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<Active> _activePool;

        public CollapseExecuteSystem(ProcessService processService)
        {
            _processService = processService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _collapseFilter = _world.Filter<CollapseRequest>().End();
            _collapsePool = _world.GetPool<CollapseRequest>();
            _processPool = _world.GetPool<CollapseProcess>();
            _targetChangedPool = _world.GetPool<TargetChanged>();
            _hexPool = _world.GetPool<Hex>();
            _activePool = _world.GetPool<Active>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _collapseFilter)
            {
                ref var hex = ref _hexPool.Get(e);
                ref var targetChanged = ref _targetChangedPool.Send();
                targetChanged.Old = hex.Target;
                hex.Target = default;

                var collapse = _collapsePool.Get(e);
                
                ref var process = ref _processService.StartNewProcess(_processPool, e, collapse.Delay * 0.05f);
                process.PlayVfx = hex.Index == 0;
                
                _collapsePool.Del(e);
                _activePool.Del(e);
            }
        }
    }
}