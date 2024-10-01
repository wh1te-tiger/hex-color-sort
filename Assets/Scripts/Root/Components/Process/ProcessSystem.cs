using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public sealed class ProcessSystem<TProcess> : IEcsInitSystem, IEcsRunSystem where TProcess : struct, IProcessData
    {
        private EcsFilter _filter;
        private EcsFilter _started;
        private EcsFilter _completed;

        private EcsPool<Process> _processPool;
        private EcsPool<HasActiveProcess> _hasProcessPool;
        private EcsPool<Started<TProcess>> _startedPool ;
        private EcsPool<Executing<TProcess>> _executingPool;
        private EcsPool<Completed<TProcess>> _completedPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<TProcess>().Inc<Process>().End();
            _started = world.Filter<Started<TProcess>>().End();
            _completed = world.Filter<Completed<TProcess>>().Inc<HasActiveProcess>().End();

            _processPool = world.GetPool<Process>();
            _hasProcessPool = world.GetPool<HasActiveProcess>();
            _startedPool = world.GetPool<Started<TProcess>>();
            _executingPool = world.GetPool<Executing<TProcess>>();
            _completedPool = world.GetPool<Completed<TProcess>>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _started)
            {
                _startedPool.Del(entity);
            }

            foreach (var entity in _completed)
            {
                ref Completed<TProcess> completed = ref _completedPool.Get(entity);
                ref HasActiveProcess activeProcess = ref _hasProcessPool.Get(entity);

                activeProcess.Process.Remove(completed.ProcessEntity);
                if (activeProcess.Process.Count == 0) _hasProcessPool.Del(entity);

                _completedPool.Del(entity);
            }

            foreach (var entity in _filter)
            {
                ref Process process = ref _processPool.Get(entity);
                var world = systems.GetWorld();

                if (process.Phase == StatePhase.Complete)
                {
                    world.DelEntity(entity);
                    continue;
                }

                if (!process.Target.Unpack(world, out var targetEntity))
                {
                    //
                    continue;
                }

                if (process.Phase == StatePhase.OnStart)
                {
                    process.Phase = StatePhase.Process;
                    _executingPool.Add(targetEntity) = new Executing<TProcess>(entity);
                }

                if (process.Paused)
                    continue;

                process.Duration -= Time.deltaTime;
                if (process.Duration <= 0)
                {
                    process.Phase = StatePhase.Complete;
                    if (_executingPool.Has(targetEntity))
                        _executingPool.Del(targetEntity);
                    _completedPool.Add(targetEntity) = new Completed<TProcess>(entity);
                }
            }
        }
    }
}