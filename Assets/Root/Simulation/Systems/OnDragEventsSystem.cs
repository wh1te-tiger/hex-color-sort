using Leopotam.EcsLite;

namespace Root
{
    public class OnDragEventsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DragService _dragService;
        
        private EcsFilter _beginDragFilter;
        private EcsFilter _endDragFilter;
        private EcsPool<BeginDragEvent> _beginDragEvents;
        private EcsPool<EndDragEvent> _endDragEvents;

        public OnDragEventsSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _beginDragFilter = world.Filter<BeginDragEvent>().End();
            _beginDragEvents = world.GetPool<BeginDragEvent>();
            _endDragFilter = world.Filter<EndDragEvent>().End();
            _endDragEvents = world.GetPool<EndDragEvent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _beginDragFilter)
            {
                ref var dragEvent = ref _beginDragEvents.Get(entity);
                var pointerData = dragEvent.PointerEventData;
                _dragService.HandleBeginDragEvent(pointerData);
            }
            
            foreach (var entity in _endDragFilter)
            {
                ref var dragEvent = ref _endDragEvents.Get(entity);
                var pointerData = dragEvent.PointerEventData;
                _dragService.HandleEndDragEvent(pointerData);
            }
        }
    }
}