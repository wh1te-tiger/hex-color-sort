using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class DelaySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _delayFilter;
        private EcsPool<Delay> _delayPool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _delayFilter = world.Filter<Delay>().End();
            _delayPool = world.GetPool<Delay>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var d in _delayFilter)
            {
                ref var delay = ref _delayPool.Get(d);
                delay.Value -= Time.deltaTime;
                if (delay.Value <= 0)
                {
                    _delayPool.Del(d);
                }
            }
        }
    }
}