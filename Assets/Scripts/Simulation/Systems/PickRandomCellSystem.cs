using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class PickRandomCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ProcessService _processService;
        
        private EcsWorld _world;
        private EcsFilter _cellFilter;
        private EcsPool<Cell> _cellPool;
        private EcsPool<Empty> _emptyPool;
        private EcsPool<ShiftRequest> _shiftRequestPool;

        public PickRandomCellSystem(ProcessService processService)
        {
            _processService = processService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cellFilter = _world.Filter<Cell>().Exc<Empty>().End();
            _cellPool = _world.GetPool<Cell>();
            _emptyPool = _world.GetPool<Empty>();
            _shiftRequestPool = _world.GetPool<ShiftRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            if(_processService.IsAnyProcess) return;

            var cells= _cellFilter
                .GetRawEntities()
                .Take(_cellFilter.GetEntitiesCount())
                .ToArray();
            if (cells.Length == 0) return;
            
            var e = cells[Random.Range(0, cells.Length)];
            
            var cell = _cellPool.Get(e);

            var emptyNeighbors = 
                cell.Neighbors.Where(n => _emptyPool.Has(n.Id)).ToArray();
            if(emptyNeighbors.Length == 0) return;
                
            var count = cell.Count;

            var from = _world.PackEntity(e);
            var to = emptyNeighbors[Random.Range(0, emptyNeighbors.Length)];
                
            var r = _world.NewEntity();
            ref var request = ref _shiftRequestPool.Add(r);
            request.Count = count;
            request.From = from;
            request.To = to;
        }
    }
}