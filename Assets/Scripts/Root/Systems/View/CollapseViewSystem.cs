using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CollapseViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;
        private readonly ViewSettings _viewSettings;

        private EcsWorld _world;
        
        private EcsFilter _collapseFilter;
        private readonly EventListener _eventListener = new();
        
        private EcsPool<Process> _processPool;
        private EcsPool<CollapseProcess> _collapsePool;
        private EcsPool<MonoLink<Transform>> _transformPool;

        public CollapseViewSystem(GameFlowService gameFlowService, ViewSettings viewSettings)
        {
            _gameFlowService = gameFlowService;
            _viewSettings = viewSettings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _collapseFilter = _world.Filter<CollapseProcess>().Exc<Delay>().End();
            _collapseFilter.AddEventListener(_eventListener);
            _processPool = _world.GetPool<Process>();
            _collapsePool = _world.GetPool<CollapseProcess>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                Process process = _processPool.Get(e);
                CollapseProcess collapsing = _collapsePool.Get(e);
                Transform transform = _transformPool.Get(process.Target.Id).Value;

                if (collapsing.PlayParticles)
                {
                    var collapse = Object.Instantiate(_viewSettings.CollapseEffect);
                    collapse.transform.position = transform.position;
                    collapse.Play();
                }
                
                transform
                    .DOScale(Vector3.zero, _viewSettings.CollapseDuration)
                    //.SetEase(Ease.OutBounce)
                    .onComplete += () =>
                {
                    transform.localScale = Vector3.one;
                    transform.gameObject.SetActive(false);
                }; 

                _gameFlowService.SetDurationToProcess(e, _viewSettings.CollapseDuration);
            }
            _eventListener.OnAdd.Clear();
        }
    }
}