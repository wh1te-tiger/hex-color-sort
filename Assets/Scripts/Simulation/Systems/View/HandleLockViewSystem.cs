using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HandleLockViewSystem : IEcsPreInitSystem, IEcsRunSystem
    {
        private EcsFilter _lockedFilter;
        private readonly EventListener _eventListener = new();

        private EcsPool<WorldPosition> _worldPosPool;
        private EcsPool<MonoLink<Colorable>> _colorablePool;
        
        public void PreInit(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _lockedFilter = world.Filter<Locked>().End();
            _lockedFilter.AddEventListener(_eventListener);
            _worldPosPool = world.GetPool<WorldPosition>();
            _colorablePool = world.GetPool<MonoLink<Colorable>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                var colorable = _colorablePool.Get(e);
                colorable.Value.Color = Color.black;
            }
            _eventListener.OnAdd.Clear();
            
            foreach (var e in _eventListener.OnRemove)
            {
                var colorable = _colorablePool.Get(e);
                colorable.Value.TweenColor(Color.white, .25f);
            }
            _eventListener.OnRemove.Clear();
        }
    }
}