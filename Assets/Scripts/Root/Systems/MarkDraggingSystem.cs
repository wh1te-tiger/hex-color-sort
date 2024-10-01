using Leopotam.EcsLite;

namespace Scripts
{
    public class MarkDraggingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _selectedFilter;
        private EcsFilter _hexFiler;
        private EcsPool<Hex> _hexPool;
        private EcsPool<Dragging> _draggingPool;
        private readonly EventListener _eventListener = new();

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _hexFiler = world.Filter<Hex>().End();
            _selectedFilter = world.Filter<Selected>().End();
            _selectedFilter.AddEventListener(_eventListener);
            
            _hexPool = world.GetPool<Hex>();
            _draggingPool = world.GetPool<Dragging>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                foreach (var eHex in _hexFiler)
                {
                    ref var hex = ref _hexPool.Get(eHex);
                    if ( hex.Target.Id != e ) continue;

                    _draggingPool.Add(eHex);
                }
            }
            
            _eventListener.OnAdd.Clear();
        }
    }
}