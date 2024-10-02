using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HandleDragEndedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DragService _dragService;

        private EcsWorld _world;
        private EcsFilter _dragEndedFilter;
        private EcsFilter _selectedFilter;
        private EcsFilter _hexFilter;
       
        private EcsPool<DragEnded> _dragEndedPool;
        private EcsPool<Selected> _selectedPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<TargetChanged> _targetChangedPool;

        public HandleDragEndedSystem(DragService dragService)
        {
            _dragService = dragService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _dragEndedFilter = _world.Filter<DragEnded>().End();
            _selectedFilter = _world.Filter<Selected>().End();
            _hexFilter = _world.Filter<Hex>().End();
            
            _dragEndedPool = _world.GetPool<DragEnded>();
            _selectedPool = _world.GetPool<Selected>();
            _hexPool = _world.GetPool<Hex>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _dragEndedFilter)
            {
                foreach (var s in _selectedFilter)
                {
                    ref var ended = ref _dragEndedPool.Get(e);
                    if (_dragService.CheckCellTarget(ended.MousePosition, out var target))
                    {
                        foreach (var h in _hexFilter)
                        {
                            ref var hex = ref _hexPool.Get(h);
                            if (!hex.Target.Unpack(_world, out _)) continue;
                            if (hex.Target.Id != s) continue;
                            
                            var oldTarget = hex.Target;
                            hex.Target = _world.PackEntity(target);
                            
                            ref var targetChanged = ref _world.Send<TargetChanged>();
                            targetChanged.New = hex.Target;
                            targetChanged.Old = oldTarget;
                        }
                    }
                    
                    _selectedPool.Del(s);
                }
                
                _dragEndedPool.Del(e);
            }
        }
    }
}