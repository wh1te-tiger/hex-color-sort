using Leopotam.EcsLite;

namespace Root
{
    public class ContainerSpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ContainerFactory _containerFactory;
        
        private EcsFilter _emptySlotsFilter;
        private EcsPool<EmptySlotsEvent> _emptySlotsPool;

        public ContainerSpawnSystem(ContainerFactory containerFactory)
        {
            _containerFactory = containerFactory;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _emptySlotsFilter = world.Filter<EmptySlotsEvent>().End();

            _emptySlotsPool = world.GetPool<EmptySlotsEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _emptySlotsFilter)
            {
                for (var i = 0; i < 3; i++)
                {
                    _containerFactory.Create();
                }
                _emptySlotsPool.Del(e);
            }
        }
    }
}