using Leopotam.EcsLite;

namespace Scripts
{
    public class MarkDraggingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _selectedFilter;
        private EcsFilter _hexFiler;
        private EcsPool<Hex> _hexPool;
        private EcsPool<Dragging> _draggingPool;
        private readonly EventListener _eventListener = new();

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _hexFiler = _world.Filter<Hex>().End();
            _selectedFilter = _world.Filter<Selected>().End();
            _selectedFilter.AddEventListener(_eventListener);
            
            _hexPool = _world.GetPool<Hex>();
            _draggingPool = _world.GetPool<Dragging>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                foreach (var eHex in _hexFiler)
                {
                    ref var hex = ref _hexPool.Get(eHex);

                    if (!hex.Target.Unpack(_world, out var target)) continue;
                    
                    if ( target == e) _draggingPool.Add(eHex);
                }
            }
            
            _eventListener.OnAdd.Clear();
        }
    }
}