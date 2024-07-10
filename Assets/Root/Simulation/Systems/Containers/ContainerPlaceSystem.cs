using Leopotam.EcsLite;

namespace Root
{
    public class ContainerPlaceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelViewService _levelViewService;
        
        private EcsFilter _initRequestFilter;
        private EcsPool<Position> _positionPool;
        private EcsPool<Parent> _parentPool;
        private EcsPool<Destination> _destinationPool;
        private EcsPool<Delay> _delayPool;

        private const float DelayOffset = 0.2f; 

        public ContainerPlaceSystem(LevelViewService levelViewService)
        {
            _levelViewService = levelViewService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().End();
            
            _positionPool = world.GetPool<Position>();
            _parentPool = world.GetPool<Parent>();
            _destinationPool = world.GetPool<Destination>();
            _delayPool = world.GetPool<Delay>();
        }

        public void Run(IEcsSystems systems)
        {
            var index = 0;
            foreach (var e in _initRequestFilter)
            {
                ref var parentComponent = ref _parentPool.Get(e);
                ref var positionComponent = ref _positionPool.Get(e);

                var slot = _levelViewService.GetFirstFreeSlotTransform();
                parentComponent.Property.SetValueAndForceNotify(slot);
                positionComponent.Property.Value = _levelViewService.SlotSpawnPos.position;
                
                if (!_destinationPool.Has(e)) _destinationPool.Add(e);
                ref var destinationComponent = ref _destinationPool.Get(e);
                destinationComponent.Value = slot.position;

                if (!_delayPool.Has(e))
                {
                    _delayPool.Add(e);
                }
                ref var delayComponent = ref _delayPool.Get(e);
                delayComponent.Value = DelayOffset * index;
                index++;
            }
        }
    }
}