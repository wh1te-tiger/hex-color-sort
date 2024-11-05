using Leopotam.EcsLite;

namespace Scripts
{
    public class CheckScoreLockConditionSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly LevelService _levelService;
        
        private EcsFilter _scoreChangedFilter;
        private EcsFilter _scoreLockConditionFilter;
        
        private EcsPool<ScoreChanged> _scoreChangedPool;
        private EcsPool<ScoreLockCondition> _scoreLockConditionPool;
        private EcsPool<Locked> _lockedPool;

        public CheckScoreLockConditionSystem(LevelService levelService)
        {
            _levelService = levelService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            _scoreChangedFilter = world.Filter<ScoreChanged>().End();
            _scoreLockConditionFilter = world.Filter<ScoreLockCondition>().End();

            _scoreChangedPool = world.GetPool<ScoreChanged>();
            _scoreLockConditionPool = world.GetPool<ScoreLockCondition>();
            _lockedPool = world.GetPool<Locked>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var _ in _scoreChangedFilter)
            {
                foreach (var e in _scoreLockConditionFilter)
                {
                    var condition = _scoreLockConditionPool.Get(e);
                    
                    if (_levelService.Score < condition.TargetScore) continue;
                    
                    _lockedPool.Del(e);
                    _scoreLockConditionPool.Del(e);
                }
                
                _scoreChangedPool.Del(_);
            }
        }
    }
}