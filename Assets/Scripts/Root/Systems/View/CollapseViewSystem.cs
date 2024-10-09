using System;
using DG.Tweening;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;
using UnityEngine.VFX;

namespace Scripts
{
    public class CollapseViewSystem : IEcsInitSystem, IEcsRunSystem, IDisposable
    {
        private readonly GameFlowService _gameFlowService;
        private readonly ViewSettings _viewSettings;
        private readonly IFactory<EntityProvider> _hexFactory;
        private readonly IFactory<VfxProvider> _vfxFactory;

        private EcsWorld _world;
        
        private EcsFilter _collapseFilter;
        private readonly EventListener _eventListener = new();
        
        private EcsPool<Process> _processPool;
        private EcsPool<CollapseProcess> _collapsePool;
        private EcsPool<MonoLink<Transform>> _transformPool;

        private readonly CompositeDisposable _disposables = new();

        public CollapseViewSystem(GameFlowService gameFlowService, ViewSettings viewSettings,
            IFactory<EntityProvider> hexFactory, IFactory<VfxProvider> vfxFactory)
        {
            _gameFlowService = gameFlowService;
            _viewSettings = viewSettings;
            _hexFactory = hexFactory;
            _vfxFactory = vfxFactory;
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

                if (collapsing.PlayVfx)
                {
                    var vfx = _vfxFactory.Create();
                    vfx.transform.position = transform.position;
                    vfx.Play();
                    vfx.Subscribe(() => _vfxFactory.Release(vfx));
                }
                
                transform
                    .DOScale(Vector3.zero, _viewSettings.CollapseDuration)
                    .onComplete += () =>
                {
                    _hexFactory.Release(transform.gameObject.GetComponent<EntityProvider>());
                }; 

                _gameFlowService.SetDurationToProcess(e, _viewSettings.CollapseDuration);
            }
            _eventListener.OnAdd.Clear();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}