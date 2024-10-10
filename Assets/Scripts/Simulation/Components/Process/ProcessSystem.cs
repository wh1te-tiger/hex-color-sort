using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public sealed class ProcessSystem<TProcess> : IEcsInitSystem, IEcsRunSystem where TProcess : struct, IProcessData
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private readonly EventListener _eventListener = new();

        private EcsPool<Process> _processPool;
        private EcsPool<TProcess> _processComponentPool;
        private EcsPool<HasActiveProcess> _hasProcessPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<TProcess>().Inc<Process>().Exc<Delay>().End();
            _filter.AddEventListener(_eventListener);

            _processPool = _world.GetPool<Process>();
            _processComponentPool = _world.GetPool<TProcess>();
            _hasProcessPool = _world.GetPool<HasActiveProcess>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref Process process = ref _processPool.Get(entity);
                
                if (process.Paused)
                    continue;

                process.Duration -= Time.deltaTime;
                if (process.Duration <= 0)
                {
                    ref HasActiveProcess activeProcess = ref _hasProcessPool.Get(process.Target.Id);

                    activeProcess.Process.Remove(entity);
                    if (activeProcess.Process.Count == 0) _hasProcessPool.Del(process.Target.Id);
                    
                    _processComponentPool.Del(entity);
                    _processPool.Del(entity);
                }
            }
        }
    }
}