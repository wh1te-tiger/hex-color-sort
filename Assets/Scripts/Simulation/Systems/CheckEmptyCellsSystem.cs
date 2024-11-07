using Leopotam.EcsLite;

namespace Scripts
{
    public class CheckEmptyCellsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelService _levelService;
        private readonly ProcessService _processService;
        
        private EcsFilter _emptyCellsFilter;

        public CheckEmptyCellsSystem(LevelService levelService, ProcessService processService)
        {
            _levelService = levelService;
            _processService = processService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _emptyCellsFilter = world.Filter<Cell>().Inc<Empty>().Exc<Locked>().End();
        }

        public void Run(IEcsSystems systems)
        {
            if(_processService.IsAnyProcess) return;
            
            if (_emptyCellsFilter.GetEntitiesCount() == 0) _levelService.SetFailedState();
        }
    }
}