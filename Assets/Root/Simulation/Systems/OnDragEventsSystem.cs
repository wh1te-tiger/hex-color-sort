using Leopotam.EcsLite;

namespace Root
{
    public class OnDragEventsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DragService _dragService;
        
        private EcsFilter _beginDragFilter;
        private EcsFilter _endDragFilter;
        private EcsFilter _selectedFilter;
        private EcsPool<BeginDragEvent> _beginDragPool;
        private EcsPool<EndDragEvent> _endDragPool;
        private EcsPool<SelectedTag> _selectedPool;

        public OnDragEventsSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _beginDragFilter = world.Filter<BeginDragEvent>().End();
            _endDragFilter = world.Filter<EndDragEvent>().End();
            _selectedFilter = world.Filter<SelectedTag>().End();
            
            _beginDragPool = world.GetPool<BeginDragEvent>();
            _endDragPool = world.GetPool<EndDragEvent>();
            _selectedPool = world.GetPool<SelectedTag>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _beginDragFilter)
            {
                ref var dragEvent = ref _beginDragPool.Get(entity);
                var pointerData = dragEvent.PointerEventData;
                _dragService.HandleBeginDragEvent(pointerData);
            }
            
            foreach (var entity in _endDragFilter)
            {
                //ref var dragEvent = ref _endDragPool.Get(entity);
                //var pointerData = dragEvent.PointerEventData;
                //_dragService.HandleEndDragEvent(pointerData);
                foreach (var e in _selectedFilter)
                {
                    _selectedPool.Del(e);
                }
            }
        }
    }
}