using Leopotam.EcsLite;

namespace Scripts
{
    public class HandleCellLockColorSystem : IEcsPreInitSystem, IEcsRunSystem
    {
        private readonly CoreViewSettings _viewSettings;
        
        private EcsFilter _lockedFilter;
        private readonly EventListener _eventListener = new();
        
        private EcsPool<MonoLink<Colorable>> _colorablePool;

        public HandleCellLockColorSystem(CoreViewSettings viewSettings)
        {
            _viewSettings = viewSettings;
        }

        public void PreInit(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _lockedFilter = world.Filter<Locked>().End();
            _lockedFilter.AddEventListener(_eventListener);
            _colorablePool = world.GetPool<MonoLink<Colorable>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                var colorable = _colorablePool.Get(e);
                colorable.Value.Color = _viewSettings.LockedCellColor;
            }
            _eventListener.OnAdd.Clear();
            
            foreach (var e in _eventListener.OnRemove)
            {
                var colorable = _colorablePool.Get(e);
                colorable.Value.TweenColor(_viewSettings.BaseCellColor, 1f);
            }
            _eventListener.OnRemove.Clear();
        }
    }
}