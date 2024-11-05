using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;

namespace Scripts
{
    public class FieldFactory
    {
        private readonly EcsWorld _world;
        
        private readonly FieldService _fieldService;
        private readonly FieldSettings _fieldSettings;
        
        public FieldFactory(EcsWorld world, FieldService fieldService, FieldSettings fieldSettings)
        {
            _world = world;
            _fieldService = fieldService;
            _fieldSettings = fieldSettings;
        }

        public void Create()
        {
            var cellPool = _world.GetPool<Cell>();
            var cellFilter = _world.Filter<Cell>().End();
            var emptyPool = _world.GetPool<Empty>();
            var lockedPool = _world.GetPool<Locked>();
            var scoreLockedConditionPool = _world.GetPool<ScoreLockCondition>();
            
            foreach (var cellData in _fieldSettings.cells)
            {
                var coordinates = cellData.coordinates;

                var e = _world.NewEntity();
                ref var cell = ref cellPool.Add(e);
                cell.FieldPosition = coordinates;
                cell.TopHexColor = ColorId.None;
                emptyPool.Add(e);
                
                if (cellData.isLocked)
                {
                    lockedPool.Add(e);
                    ref var condition = ref scoreLockedConditionPool.Add(e);
                    condition.TargetScore = cellData.lockCondition;
                }
                
                _fieldService.RegisterCell(e, coordinates);
            }

            
            foreach (var e in cellFilter)
            {
                ref var cell = ref cellPool.Get(e);
                List<EcsPackedEntity> n = new();
                if (_fieldService.TryGetNeighbors(cell.FieldPosition, out var neighbors))
                {
                    n.AddRange(neighbors.Select(pos => _world.PackEntity(_fieldService.GetCellEntity(pos))));
                }

                cell.Neighbors = n.ToArray();
            }
        }
    }
}