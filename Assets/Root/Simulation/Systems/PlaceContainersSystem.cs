using Leopotam.EcsLite;

namespace Root
{
    public class PlaceContainersSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelViewService _levelViewService;
        
        private EcsFilter _initRequestFilter;
        private EcsPool<InitRequest> _initRequestPool;
        private EcsPool<Position> _positionPool;
        private EcsPool<Parent> _parentPool;
        private EcsPool<Destination> _destinationPool;

        public PlaceContainersSystem(LevelViewService levelViewService)
        {
            _levelViewService = levelViewService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().End();

            _initRequestPool = world.GetPool<InitRequest>();
            _positionPool = world.GetPool<Position>();
            _parentPool = world.GetPool<Parent>();
            _destinationPool = world.GetPool<Destination>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _initRequestFilter)
            {
                ref var parentComponent = ref _parentPool.Get(e);
                ref var positionComponent = ref _positionPool.Get(e);
                if (!_destinationPool.Has(e))
                {
                    _destinationPool.Add(e);
                }
                ref var destinationComponent = ref _destinationPool.Get(e);

                var slot = _levelViewService.GetFirstFreeSlotTransform();
                parentComponent.Property.SetValueAndForceNotify(slot);
                positionComponent.Property.Value = _levelViewService.SlotSpawnPos.position;
                destinationComponent.Value = slot.position;
            }
        }
    }
}