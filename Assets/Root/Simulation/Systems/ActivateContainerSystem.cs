using Leopotam.EcsLite;

namespace Root
{
    public class ActivateContainerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _initRequestFilter;
        private EcsPool<InitRequest> _initRequestPool;
        private EcsPool<Active> _activePool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().Inc<Active>().End();

            _initRequestPool = world.GetPool<InitRequest>();
            _activePool = world.GetPool<Active>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _initRequestFilter)
            {
                ref var activeComponent = ref _activePool.Get(e);
                activeComponent.Property.Value = true;
                _initRequestPool.Del(e);
            }
        }
    }
}