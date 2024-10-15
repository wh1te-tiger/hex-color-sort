using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class TargetChangedEventSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _targetChangedFilter;
        
        private EcsPool<TargetChanged> _targetChangedPool;
        private EcsPool<Cell> _cellsPool;
        private EcsPool<Slot> _slotsPool;
        private EcsPool<Empty> _emptyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _targetChangedFilter = world.Filter<TargetChanged>().End();
            _targetChangedPool = world.GetPool<TargetChanged>();
            _cellsPool = world.GetPool<Cell>();
            _slotsPool = world.GetPool<Slot>();
            _emptyPool = world.GetPool<Empty>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _targetChangedFilter)
            {
                var targetChanged = _targetChangedPool.Get(e);

                if (targetChanged.Old.Unpack(systems.GetWorld(), out var target))
                {
                    if (_cellsPool.Has(target))
                    {
                        ref var oldCell = ref _cellsPool.Get(target);
                        oldCell.Count--;
                        if(oldCell.Count < 0) Debug.LogError("COUNT");
                        
                        if (oldCell.Count == 0)
                        {
                            _emptyPool.Add(target);
                            oldCell.TopHexColor = ColorId.None;
                        }
                    }
                    
                    if (_slotsPool.Has(target) && !_emptyPool.Has(target))
                    {
                        _emptyPool.Add(target);
                    }
                }
                
                if (targetChanged.New.Unpack(systems.GetWorld(), out target) )
                {
                    if (_cellsPool.Has(target))
                    {
                        ref var cell = ref _cellsPool.Get(target);
                        if (cell.Count == 0)
                        {
                            _emptyPool.Del(target);
                        }
                        cell.Count++;
                    }

                    if (_slotsPool.Has(target) && _emptyPool.Has(target))
                    {
                        _emptyPool.Del(target);
                    }
                }
                
                _targetChangedPool.Del(e);
            }
        }
    }
}