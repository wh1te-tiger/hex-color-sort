using Leopotam.EcsLite;

namespace Scripts
{
    public class HandleDragStartedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DragService _dragService;
        
        private EcsFilter _dragStartedFilter;
        private EcsPool<DragStarted> _dragStartedPool;
        private EcsPool<Selected> _selectedPool;

        public HandleDragStartedSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _dragStartedFilter = world.Filter<DragStarted>().End();
            _dragStartedPool = world.GetPool<DragStarted>();
            
            _selectedPool = world.GetPool<Selected>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _dragStartedFilter)
            {
                ref var started = ref _dragStartedPool.Get(e);
                if (_dragService.CheckSlotTarget(started.MousePosition, out var target))
                {
                    _selectedPool.Add(target);
                }
                
                _dragStartedPool.Del(e);
            }
        }
    }
}