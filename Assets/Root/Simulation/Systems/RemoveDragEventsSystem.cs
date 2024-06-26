using Leopotam.EcsLite;

namespace Root
{
    public class RemoveDragEventsSystem : IEcsInitSystem, IEcsPostRunSystem
    {
        private EcsFilter _beginDragFilter;
        private EcsFilter _endDragFilter;
        private EcsPool<BeginDragEvent> _beginDragEvents;
        private EcsPool<EndDragEvent> _endDragEvents;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _beginDragFilter = world.Filter<BeginDragEvent>().End();
            _beginDragEvents = world.GetPool<BeginDragEvent>();
            _endDragFilter = world.Filter<EndDragEvent>().End();
            _endDragEvents = world.GetPool<EndDragEvent>();
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var entity in _beginDragFilter)
            {
                _beginDragEvents.Del(entity);
            }
            
            foreach (var entity in _endDragFilter)
            {
                _endDragEvents.Del(entity);
            }
        }
    }
}