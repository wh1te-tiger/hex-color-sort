using System;
using Leopotam.EcsLite;

namespace Scripts
{
    public class GameFlowService : IDisposable
    {
        public bool IsAnyoneActing => _acting.GetEntitiesCount() > 0;
        public bool IsHexCreationNeeded => _emptySlots.GetEntitiesCount() == 3;
        public bool IsDragging => _dragging.GetEntitiesCount() == 1;
        public int Score { get; private set; }
        
        private readonly EcsPool<HasActiveProcess> _activeProcessPool;
        private readonly EcsPool<Process> _processPool;
        private readonly EcsPool<Delay> _delayPool;
        
        private readonly EcsFilter _acting;
        private readonly EcsFilter _emptySlots;
        private readonly EventListener _eventListener = new();
        private readonly EcsFilter _dragging;
        
        public GameFlowService(EcsWorld w)
        {
            _acting = w.Filter<HasActiveProcess>().End();
            _emptySlots = w.Filter<Slot>().Inc<Empty>().End();
            _dragging = w.Filter<Slot>().Inc<Selected>().End();
            var collapsedFilter = w.Filter<CollapseRequest>().End();
            collapsedFilter.AddEventListener(_eventListener);
            _eventListener.OnAdded += IncreaseScore;
            
            _processPool = w.GetPool<Process>();
            _activeProcessPool = w.GetPool<HasActiveProcess>();
            _delayPool = w.GetPool<Delay>();
        }
        
        private void IncreaseScore()
        {
            Score++;
        }

        #region Process

        public ref TProcess StartNewProcess<TProcess>(EcsPool<TProcess> pool, int entity) where TProcess : struct, IProcessData
        {
            var world = pool.GetWorld();
            var processEntity = world.NewEntity();
            ref var process = ref _processPool.Add(processEntity);
            process.Target = world.PackEntity(entity);
            ref var activeProc = ref _activeProcessPool.GetOrAdd(entity);
            activeProc.Process.Add(processEntity);
            
            return ref pool.Add(processEntity);
        }
        
        public ref TProcess StartNewProcess<TProcess>(EcsPool<TProcess> pool, int entity, float delay) where TProcess : struct, IProcessData
        {
            var world = pool.GetWorld();
            var processEntity = world.NewEntity();
            ref var process = ref _processPool.Add(processEntity);
            process.Target = world.PackEntity(entity);
            
            ref var activeProc = ref _activeProcessPool.GetOrAdd(entity);
            activeProc.Process.Add(processEntity);
            
            ref var delayComponent = ref _delayPool.Add(processEntity);
            delayComponent.Value = delay;
            
            delayComponent = ref _delayPool.Add(entity);
            delayComponent.Value = delay;
            
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

        #region Disposable

        public void Dispose()
        {
            _eventListener.OnAdded -= IncreaseScore;
        }

        #endregion
    }
}