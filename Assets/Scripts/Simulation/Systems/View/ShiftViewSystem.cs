using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class ShiftViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;
        private readonly ViewSettings _viewSettings;

        private EcsWorld _world;
        
        private EcsFilter _shiftFilter;
        private readonly EventListener _eventListener = new();
        private EcsPool<Process> _processPool;
        private EcsPool<ShiftProcess> _shiftPool;
        private EcsPool<MonoLink<Transform>> _transformPool;
        private EcsPool<WorldPosition> _worldPosPool;

        public ShiftViewSystem(GameFlowService gameFlowService, ViewSettings viewSettings)
        {
            _gameFlowService = gameFlowService;
            _viewSettings = viewSettings;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _shiftFilter = _world.Filter<ShiftProcess>().Exc<Delay>().End();
            _shiftFilter.AddEventListener(_eventListener);
            _processPool = _world.GetPool<Process>();
            _shiftPool = _world.GetPool<ShiftProcess>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
            _worldPosPool = _world.GetPool<WorldPosition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                Process process = _processPool.Get(e);
                ShiftProcess shifting = _shiftPool.Get(e);
                Transform transform = _transformPool.Get(process.Target.Id).Value;

                if (shifting.Target.Unpack(_world, out var target))
                {
                    var targetPos = _worldPosPool.Get(target).Value;
                    var direction = (targetPos - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
                    //TODO: move to settings
                    targetPos = new Vector3(targetPos.x, shifting.Height * (_viewSettings.HexSpacing + _viewSettings.HexHeight), targetPos.z);

                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                    var sequence = DOTween.Sequence();
                    sequence
                        //TODO: move to settings
                        .Append(transform.DOJump(targetPos, 1, 1, _viewSettings.ShiftDuration))
                        .Join(transform.DORotate(new Vector3(180, transform.rotation.eulerAngles.y, 0), _viewSettings.ShiftDuration,
                            RotateMode.FastBeyond360))
                        .AppendCallback(() =>
                        {
                            transform.rotation = Quaternion.identity;
                        });
                
                    _gameFlowService.SetDurationToProcess(e, sequence.Duration());
                }
            }
            _eventListener.OnAdd.Clear();
        }
    }
}