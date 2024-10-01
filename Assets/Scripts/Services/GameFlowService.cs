using Leopotam.EcsLite;

namespace Scripts
{
    public class GameFlowService
    {
        private readonly EcsPool<HasActiveProcess> _activeProcessPool;
        private readonly EcsPool<Process> _processPool;
        private readonly EcsFilter _acting;
        private readonly EcsFilter _emptySlots;

        public bool IsAnyoneActing => _acting.GetEntitiesCount() > 0;
        public bool IsHexCreationNeeded => _emptySlots.GetEntitiesCount() == 3;

        public GameFlowService(EcsWorld w)
        {
            _processPool = w.GetPool<Process>();
            _activeProcessPool = w.GetPool<HasActiveProcess>();
            _acting = w.Filter<HasActiveProcess>().End();
            _emptySlots = w.Filter<Slot>().Inc<Empty>().End();
        }

        #region Process

        public ref TProcess StartNewProcess<TProcess>(EcsPool<TProcess> pool, int entity) where TProcess : struct, IProcessData
        {
            var world = pool.GetWorld();
            var processEntity = world.NewEntity();
            ref var process = ref _processPool.Add(processEntity);
            process.Phase = StatePhase.OnStart;
            process.Target = world.PackEntity(entity);
            ref var activeProc = ref _activeProcessPool.GetOrAdd(entity);
            activeProc.Process.Add(processEntity);
            
            world.GetPool<Started<TProcess>>().Add(entity) = new Started<TProcess>(processEntity);
            return ref pool.Add(processEntity);
        }
        
        public void PauseProcess(int processEntity)
        {
            _processPool.Get(processEntity).Paused = true;
        }
        
        public void UnpauseProcess(int processEntity)
        {
            _processPool.Get(processEntity).Paused = false;
        }
        
        public void SetDurationToProcess(int processEntity, float duration)
        {
            _processPool.Get(processEntity).Duration = duration;
        }

        #endregion
    }
}