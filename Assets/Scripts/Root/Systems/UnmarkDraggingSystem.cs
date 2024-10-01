using Leopotam.EcsLite;

namespace Scripts
{
    public class UnmarkDraggingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _selectedFilter;
        private EcsFilter _hexFiler;
        private EcsPool<Dragging> _draggingPool;
        private readonly EventListener _eventListener = new();

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _hexFiler = world.Filter<Hex>().End();
            _selectedFilter = world.Filter<Selected>().End();
            _selectedFilter.AddEventListener(_eventListener);
            
            _draggingPool = world.GetPool<Dragging>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var _ in _eventListener.OnRemove)
            {
                foreach (var eHex in _hexFiler)
                {
                    if (!_draggingPool.Has(eHex)) continue;
                    
                    _draggingPool.Del(eHex);
                }
            }
            _eventListener.OnRemove.Clear();
        }
    }
}