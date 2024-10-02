using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CollapseViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;

        private EcsWorld _world;
        
        private EcsFilter _collapseFilter;
        private EcsPool<Started<CollapseProcess>> _startedPool;
        private EcsPool<CollapseProcess> _collapsePool;
        private EcsPool<MonoLink<Transform>> _transformPool;
        private EcsPool<WorldPosition> _worldPosPool;

        public CollapseViewSystem(GameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _collapseFilter = _world.Filter<Started<CollapseProcess>>().Exc<Delay>().End();
            _startedPool = _world.GetPool<Started<CollapseProcess>>();
            _collapsePool = _world.GetPool<CollapseProcess>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
            _worldPosPool = _world.GetPool<WorldPosition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _collapseFilter)
            {
                Started<CollapseProcess> processLink = _startedPool.Get(e);
                CollapseProcess collapsing = processLink.GetProcessData(_collapsePool);
                Transform transform = _transformPool.Get(e).Value;

                transform
                    .DOScale(Vector3.zero, 0.1f)
                    .SetEase(Ease.OutBounce)
                    .onComplete += () =>
                {
                    transform.localScale = Vector3.one;
                    transform.gameObject.SetActive(false);
                }; 

                _gameFlowService.SetDurationToProcess(processLink.ProcessEntity, 0.1f);
            }
        }
    }
}