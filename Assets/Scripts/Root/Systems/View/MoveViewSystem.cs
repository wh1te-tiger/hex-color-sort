using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class MoveViewSystem : IEcsInitSystem, IEcsRunSystem 
    {
        private readonly GameFlowService _gameFlowService;

        private EcsWorld _world;
        
        private EcsFilter _moveFilter;
        private EcsPool<Started<MoveProcess>> _startedPool;
        private EcsPool<MoveProcess> _movePool;
        private EcsPool<MonoLink<Transform>> _transformPool;
        private EcsPool<WorldPosition> _worldPosPool;
        private EcsPool<HeightOffset> _offsetPool;

        public MoveViewSystem(GameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _moveFilter = _world.Filter<Started<MoveProcess>>().Inc<MonoLink<Transform>>().End();
            _startedPool = _world.GetPool<Started<MoveProcess>>();
            _movePool = _world.GetPool<MoveProcess>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
            _worldPosPool = _world.GetPool<WorldPosition>();
            _offsetPool = _world.GetPool<HeightOffset>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var e in _moveFilter)
            {
                Started<MoveProcess> processLink = _startedPool.Get(e);
                MoveProcess moving = processLink.GetProcessData(_movePool);
                Transform transform = _transformPool.Get(e).Value;

                if (moving.Target.Unpack(_world, out var target))
                {
                    var offset = 0f;
                    if (_offsetPool.Has(target))
                    {
                        offset = _offsetPool.Get(target).Value;
                    }
                    
                    var targetPos = _worldPosPool.Get(target).Value;
                    targetPos += Vector3.up * (transform.position.y + offset);
                    
                    var distance = Vector3.Distance(transform.position, targetPos);
                    var duration = distance / 30f;
                    
                    transform.DOMove(targetPos, duration);

                    _gameFlowService.SetDurationToProcess(processLink.ProcessEntity, duration + moving.Delay);
                }
            }
        }
    }
}