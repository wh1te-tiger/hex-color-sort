using Leopotam.EcsLite;

namespace Root
{
    public class ContainerDeactivateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _unloadRequestFilter;
        private EcsPool<UnloadRequest> _unloadRequestPool;
        private EcsPool<Active> _activePool;
        
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _unloadRequestFilter = world.Filter<UnloadRequest>().Inc<Active>().End();

            _unloadRequestPool = world.GetPool<UnloadRequest>();
            _activePool = world.GetPool<Active>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _unloadRequestFilter)
            {
                ref var activeComponent = ref _activePool.Get(e);
                activeComponent.Property.Value = false;
                _unloadRequestPool.Del(e);
            }
        }
    }
}