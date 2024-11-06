using Leopotam.EcsLite;

namespace Scripts
{
    public class HandleCellLockIconSystem : IEcsPreInitSystem, IEcsRunSystem
    {
        private readonly LockedOverlayService _lockedOverlayService;
        
        private EcsFilter _lockedFilter;
        private readonly EventListener _eventListener = new();

        private EcsPool<WorldPosition> _worldPosPool;
        private EcsPool<ScoreLockCondition> _lockConditionPool;

        public HandleCellLockIconSystem(LockedOverlayService lockedOverlayService)
        {
            _lockedOverlayService = lockedOverlayService;
        }


        public void PreInit(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _lockedFilter = world.Filter<Locked>().Inc<WorldPosition>().Inc<ScoreLockCondition>().End();
            _lockedFilter.AddEventListener(_eventListener);
            _worldPosPool = world.GetPool<WorldPosition>();
            _lockConditionPool = world.GetPool<ScoreLockCondition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                var pos = _worldPosPool.Get(e);
                var condition = _lockConditionPool.Get(e);
                _lockedOverlayService.AddLock(e, pos.Value, condition.TargetScore);
            }
            _eventListener.OnAdd.Clear();
            
            foreach (var e in _eventListener.OnRemove)
            {
                _lockedOverlayService.RemoveLock(e);
            }
            _eventListener.OnRemove.Clear();
        }
    }
}