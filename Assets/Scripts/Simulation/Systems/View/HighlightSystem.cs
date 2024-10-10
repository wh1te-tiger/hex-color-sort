using Leopotam.EcsLite;

namespace Scripts
{
    public class HighlightSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly CoreViewSettings _coreViewSettings;

        private EcsWorld _world;
        
        private EcsFilter _cellFilter;
        private readonly EventListener _eventListener = new();
        
        private EcsPool<MonoLink<Colorable>> _colorablePool;
        
        public HighlightSystem(CoreViewSettings coreViewSettings)
        {
            _coreViewSettings = coreViewSettings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _cellFilter = _world.Filter<Cell>().Inc<Selected>().End();
            _cellFilter.AddEventListener(_eventListener);
            
            _colorablePool = _world.GetPool<MonoLink<Colorable>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                var colorable = _colorablePool.Get(e);
                colorable.Value.Color = _coreViewSettings.HighlightedCellColor;
            }
            _eventListener.OnAdd.Clear();
            
            foreach (var e in _eventListener.OnRemove)
            {
                var colorable = _colorablePool.Get(e);
                colorable.Value.Color = _coreViewSettings.BaseCellColor;
            }
            _eventListener.OnRemove.Clear();
        }
    }
}