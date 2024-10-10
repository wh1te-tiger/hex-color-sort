using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class PickRandomCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly FieldService _fieldService;
        private readonly HexService _hexService;
        private readonly GameFlowService _gameFlowService;
        
        private EcsWorld _world;
        private EcsFilter _cellFilter;
        private EcsPool<Cell> _cellPool;
        private EcsPool<ShiftRequest> _shiftRequestPool;

        public PickRandomCellSystem(FieldService fieldService, HexService hexService, GameFlowService gameFlowService)
        {
            _fieldService = fieldService;
            _hexService = hexService;
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cellFilter = _world.Filter<Cell>().Inc<WorldPosition>().Exc<Empty>().End();
            _cellPool = _world.GetPool<Cell>();
            _shiftRequestPool = _world.GetPool<ShiftRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            if(_gameFlowService.IsAnyoneActing) return;

            var entitiesCount = _cellFilter.GetEntitiesCount();
            var filledCells = _cellFilter
                .GetRawEntities()
                .Take(entitiesCount)
                .Select(e => _cellPool.Get(e))
                .ToArray();

            var cell = filledCells[Random.Range(0, filledCells.Length)].FieldPosition;

            if (!_fieldService.TryGetNeighbors(cell, out var neighbors)) return;

            var emptyNeighbors = neighbors.Where(n => !filledCells.Select(c => c.FieldPosition).Contains(n)).ToArray();
            
            var neighborPos = emptyNeighbors[Random.Range(0, emptyNeighbors.Length)];
            
            var count = _hexService.GetTopHexColorCount(_fieldService.GetCellEntity(cell));

            var from = _world.PackEntity(_fieldService.GetCellEntity(cell));
            var to = _world.PackEntity(_fieldService.GetCellEntity(neighborPos));
            
            var r = _world.NewEntity();
            ref var request = ref _shiftRequestPool.Add(r);
            request.Count = count;
            request.From = from;
            request.To = to;
        }
    }
}