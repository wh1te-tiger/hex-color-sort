using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class DelaySystem : IEcsInitSystem, IEcsFixedRunSystem
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
            foreach (var e in _delayFilter)
            {
                ref var delayComponent = ref _delayPool.Get(e);
                delayComponent.Value -= Time.fixedDeltaTime;
                
                if (delayComponent.Value <= 0)
                {
                    _delayPool.Del(e);
                }
            }
        }
    }
}