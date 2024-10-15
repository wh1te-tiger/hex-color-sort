using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class PickCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly HexService _hexService;
        private readonly ProcessService _processService;
        
        private EcsWorld _world;
        private EcsFilter _cellFilter;
        private EcsPool<Cell> _cellPool;
        private EcsPool<ShiftRequest> _shiftRequestPool;
        private EcsPool<Empty> _emptyPool;

        public PickCellSystem(HexService hexService, ProcessService processService)
        {
            _hexService = hexService;
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
            
            foreach (var e in _cellFilter)
            {
                var cell = _cellPool.Get(e);

                var notEmptyNeighbors = 
                    cell.Neighbors.Where(n => !_emptyPool.Has(n.Id)).ToArray();
                if(notEmptyNeighbors.Length == 0) continue;
                
                var sameColorNeighbors =
                    notEmptyNeighbors.Where(n => _cellPool.Get(n.Id).TopHexColor == cell.TopHexColor).ToArray();
                
                if(sameColorNeighbors.Length == 0) continue;

                EcsPackedEntity from;
                EcsPackedEntity to;
                int count;

                if (sameColorNeighbors.Length == 1)
                {
                    count = _hexService.GetTopColorHexCount(e);
                    
                    from = _world.PackEntity(e);
                    to = sameColorNeighbors[0];
                }
                else
                {
                    var index = Random.Range(0, sameColorNeighbors.Length);
                    count = _hexService.GetTopColorHexCount(sameColorNeighbors[index].Id);

                    from = sameColorNeighbors[index];
                    to = _world.PackEntity(e);
                }
                
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