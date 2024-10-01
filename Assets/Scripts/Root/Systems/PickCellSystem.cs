using System.Linq;
using Leopotam.EcsLite;

namespace Scripts
{
    public class PickCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly FieldService _fieldService;
        private readonly HexService _hexService;
        private readonly GameFlowService _gameFlowService;
        
        private EcsWorld _world;
        private EcsFilter _cellFilter;
        private EcsPool<Cell> _cellPool;
        private EcsPool<ShiftRequest> _shiftRequestPool;

        public PickCellSystem(FieldService fieldService, HexService hexService, GameFlowService gameFlowService)
        {
            _fieldService = fieldService;
            _hexService = hexService;
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cellFilter = _world.Filter<Cell>().Exc<Empty>().End();
            _cellPool = _world.GetPool<Cell>();
            _shiftRequestPool = _world.GetPool<ShiftRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _cellFilter)
            {
                if(_gameFlowService.IsAnyoneActing) return;
                
                var cell = _cellPool.Get(e);
                var pos = cell.FieldPosition;

                if (!_fieldService.TryGetNeighbors(pos, out var neighbors)) continue;

                var cellEntity = _fieldService.GetCellEntity(pos);
                var color = _hexService.GetTopHexColor(cellEntity);

                var sameColor = neighbors.Where(n => _hexService.GetTopHexColor(_fieldService.GetCellEntity(n)) == color).ToArray();
                if (sameColor.Length == 0) continue;

                var count = _hexService.GetTopHexColorCount(cellEntity);
                
                var neighborPos = sameColor.First();
                
                var from = _world.PackEntity(cellEntity);
                var to = _world.PackEntity(_fieldService.GetCellEntity(neighborPos));
                
                var r = _world.NewEntity();
                ref var request = ref _shiftRequestPool.Add(r);
                request.Count = count;
                request.From = from;
                request.To = to;
                
                break;
            }
        }
    }
}