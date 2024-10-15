using Leopotam.EcsLite;

namespace Scripts
{
    public class ScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelService _levelService;
        
        private readonly EventListener _eventListener = new();

        public ScoreSystem(LevelService levelService)
        {
            _levelService = levelService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var collapsedFilter = world.Filter<CollapseRequest>().Exc<Delay>().End();
            collapsedFilter.AddEventListener(_eventListener);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var _ in _eventListener.OnAdd)
            {
                _levelService.IncreaseScore();
            }
            _eventListener.OnAdd.Clear();
        }
    }
}