using Leopotam.EcsLite;

namespace Scripts
{
    public class MoveExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;
        
        private EcsFilter _draggingFilter;
        private readonly EventListener _eventListener = new();

        private EcsPool<MoveProcess> _moveProcessPool;
        private EcsPool<Hex> _hexPool;

        public MoveExecuteSystem(GameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _draggingFilter = world.Filter<Dragging>().End();
            _draggingFilter.AddEventListener(_eventListener);

            _hexPool = world.GetPool<Hex>();
            _moveProcessPool = world.GetPool<MoveProcess>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnRemove)
            {
                var hex = _hexPool.Get(e);
                ref var process = ref _gameFlowService.StartNewProcess(_moveProcessPool, e);
                process.Target = hex.Target;
            }
            _eventListener.OnRemove.Clear();
        }
    }
}