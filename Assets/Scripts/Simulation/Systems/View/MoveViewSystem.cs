using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class MoveViewSystem : IEcsInitSystem, IEcsRunSystem 
    {
        private readonly ProcessService _processService;

        private EcsWorld _world;
        
        private EcsFilter _moveFilter;
        private readonly EventListener _eventListener = new();
        private EcsPool<Started<MoveProcess>> _startedPool;
        private EcsPool<MoveProcess> _movePool;
        private EcsPool<Process> _processPool;
        private EcsPool<MonoLink<Transform>> _transformPool;

        public MoveViewSystem(ProcessService processService)
        {
            _processService = processService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _moveFilter = _world.Filter<MoveProcess>().Exc<Delay>().End();
            _moveFilter.AddEventListener(_eventListener);
            _processPool = _world.GetPool<Process>();
            _movePool = _world.GetPool<MoveProcess>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                Process process = _processPool.Get(e);
                MoveProcess moving =  _movePool.Get(e);
                Transform transform = _transformPool.Get(process.Target.Id).Value;
                
                var targetPos = transform.position + moving.Offset;
                var offsetX = moving.Offset.x;
                var offsetY = moving.Offset.y;
                var offsetZ = moving.Offset.z;
                var distance = Vector3.Distance(transform.position, targetPos);
                var duration = distance / moving.Speed;

                if (offsetX != 0)
                {
                    transform.DOMoveX(targetPos.x, duration);
                }
                if (offsetY != 0)
                {
                    transform.DOMoveY(targetPos.y, duration);
                }
                if (offsetZ != 0)
                {
                    transform.DOMoveZ(targetPos.z, duration);
                }
                
                _processService.SetDurationToProcess(e, duration);
            }
            
            _eventListener.OnAdd.Clear();
        }
    }
}