using DG.Tweening;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class ShiftViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;

        private EcsWorld _world;
        
        private EcsFilter _shiftFilter;
        private EcsPool<Started<ShiftProcess>> _startedPool;
        private EcsPool<ShiftProcess> _shiftPool;
        private EcsPool<MonoLink<Transform>> _transformPool;
        private EcsPool<WorldPosition> _worldPosPool;

        public ShiftViewSystem(GameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _shiftFilter = _world.Filter<Started<ShiftProcess>>().Inc<MonoLink<Transform>>().Exc<Delay>().End();
            _startedPool = _world.GetPool<Started<ShiftProcess>>();
            _shiftPool = _world.GetPool<ShiftProcess>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
            _worldPosPool = _world.GetPool<WorldPosition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _shiftFilter)
            {
                Started<ShiftProcess> processLink = _startedPool.Get(e);
                ShiftProcess shifting = processLink.GetProcessData(_shiftPool);
                Transform transform = _transformPool.Get(e).Value;

                if (shifting.Target.Unpack(_world, out var target))
                {
                    var targetPos = _worldPosPool.Get(target).Value;
                    var direction = (targetPos - new Vector3(transform.position.x, 0, transform.position.z)).normalized;
                    //TODO: move to settings
                    targetPos = new Vector3(targetPos.x, shifting.Height * 0.3f, targetPos.z);

                    transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                    var sequence = DOTween.Sequence();
                    sequence
                        //TODO: move to settings
                        .Append(transform.DOJump(targetPos, 1, 1, 0.15f))
                        .Join(transform.DORotate(new Vector3(180, transform.rotation.eulerAngles.y, 0), 0.15f,
                            RotateMode.FastBeyond360))
                        .AppendCallback(() =>
                        {
                            transform.rotation = Quaternion.identity;
                        });
                
                    _gameFlowService.SetDurationToProcess(processLink.ProcessEntity, sequence.Duration());
                }
            }
        }
    }
}