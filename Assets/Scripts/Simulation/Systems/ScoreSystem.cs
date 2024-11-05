using Leopotam.EcsLite;

namespace Scripts
{
    public class ScoreSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelService _levelService;

        private EcsWorld _world;
        private readonly EventListener _eventListener = new();

        public ScoreSystem(LevelService levelService)
        {
            _levelService = levelService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            var collapsedFilter = _world.Filter<CollapseRequest>().Exc<Delay>().End();
            collapsedFilter.AddEventListener(_eventListener);
        }

        public void Run(IEcsSystems systems)
        {
            var flag = false;
            foreach (var _ in _eventListener.OnAdd)
            {
                _levelService.IncreaseScore();
                flag = true;
            }
            _eventListener.OnAdd.Clear();

            if (flag)
            {
                _world.Send<ScoreChanged>();
            }
        }
    }
}