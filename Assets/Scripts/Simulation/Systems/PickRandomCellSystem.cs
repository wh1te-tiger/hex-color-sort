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
        private EcsPool<Empty> _emptyPool;
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
            _emptyPool = _world.GetPool<Empty>();
            _shiftRequestPool = _world.GetPool<ShiftRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            if(_gameFlowService.IsAnyoneActing) return;

            var e = _cellFilter.GetRawEntities().Take(_cellFilter.GetEntitiesCount()).ToArray();
            if(e.Length == 0) return;
            
            var random = e[Random.Range(0, e.Length)];
            
            var pos = _cellPool.Get(random).FieldPosition;
                
            if (!_fieldService.TryGetNeighbors(pos, out var neighbors)) return;

            var emptyNeighbors= neighbors
                .Select(n => _fieldService.GetCellEntity(n))
                .Where(c => _emptyPool.Has(c))
                .ToArray();
                
            var target = emptyNeighbors[Random.Range(0, emptyNeighbors.Length)];
            
            var count = _hexService.GetTopHexColorCount(random);
            var from = _world.PackEntity(random);
            var to = _world.PackEntity(target);
            
            var r = _world.NewEntity();
            ref var request = ref _shiftRequestPool.Add(r);
            request.Count = count;
            request.From = from;
            request.To = to;
        }
    }
}