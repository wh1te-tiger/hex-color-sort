using System;
using Leopotam.EcsLite;

namespace Scripts
{
    public class HandleDragEndedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        
        private EcsFilter _dragEndedFilter;
        private EcsFilter _activeSlotFilter;
        private EcsFilter _selectedCellFilter;
        private EcsFilter _hexFilter;
        
       
        private EcsPool<DragEnded> _dragEndedPool;
        private EcsPool<Selected> _selectedPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<TargetChanged> _targetChangedPool;
        private EcsPool<TopHex> _topHexPool;


        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _dragEndedFilter = _world.Filter<DragEnded>().End();
            
            _activeSlotFilter = _world.Filter<Selected>().Inc<Slot>().End();
            _selectedCellFilter = _world.Filter<Selected>().Inc<Cell>().End();
            _hexFilter = _world.Filter<Hex>().End();
            
            _dragEndedPool = _world.GetPool<DragEnded>();
            _selectedPool = _world.GetPool<Selected>();
            _hexPool = _world.GetPool<Hex>();
            _topHexPool = _world.GetPool<TopHex>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _dragEndedFilter)
            {
                _dragEndedPool.Del(e);

                foreach (var s in _activeSlotFilter)
                {
                    if(_activeSlotFilter.GetEntitiesCount() > 1) 
                        throw new Exception("Invalid count of active slots!");
                    
                    _selectedPool.Del(s);
                    
                    if (_selectedCellFilter.GetEntitiesCount() > 1)
                    {
                        throw new Exception("There are more than one selected cell!");
                    }
                
                    if(_selectedCellFilter.GetEntitiesCount() == 0) continue;
                
                    var cellEntity = _selectedCellFilter.GetRawEntities()[0];
                    _selectedPool.Del(cellEntity);

                    var maxIndex = 0;
                    var hexEntity = -1;
                    foreach (var h in _hexFilter)
                    {
                        ref var hex = ref _hexPool.Get(h);
                        if (!hex.Target.Unpack(_world, out var target)) continue;
                        if (target != s) continue;

                        if (hex.Index >= maxIndex)
                        {
                            maxIndex = hex.Index;
                            hexEntity = h;
                        }
                        
                        var oldTarget = hex.Target;
                        hex.Target = _world.PackEntity(cellEntity);
                            
                        ref var targetChanged = ref _world.Send<TargetChanged>();
                        targetChanged.New = hex.Target;
                        targetChanged.Old = oldTarget;
                    }

                    _topHexPool.Add(hexEntity);
                }
            }
        }
    }
}