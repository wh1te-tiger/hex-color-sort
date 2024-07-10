using Leopotam.EcsLite;

namespace Root
{
    public class ContainerActivateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _initRequestFilter;
        private EcsPool<InitRequest> _initRequestPool;
        private EcsPool<Active> _activePool;
        private EcsPool<Organized> _organizedPool;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().Inc<Active>().End();

            _initRequestPool = world.GetPool<InitRequest>();
            _activePool = world.GetPool<Active>();
            _organizedPool = world.GetPool<Organized>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _initRequestFilter)
            {
                ref var activeComponent = ref _activePool.Get(e);
                activeComponent.Property.Value = true;
                _initRequestPool.Del(e);
                _organizedPool.Del(e);
            }
        }
    }
}