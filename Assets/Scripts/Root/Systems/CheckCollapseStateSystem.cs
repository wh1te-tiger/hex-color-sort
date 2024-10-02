using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CheckCollapseStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly HexService _hexService;
        private readonly GameFlowService _gameFlowService;
        
        private EcsWorld _world;
        
        private EcsFilter _cellFilter;
        private EcsFilter _hexFilter;
        
        private EcsPool<Hex> _hexPool;
        private EcsPool<Cell> _cellPool;
        private EcsPool<CollapseRequest> _collapseRequestPool;

        public CheckCollapseStateSystem(GameFlowService gameFlowService, HexService hexService)
        {
            _hexService = hexService;
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cellFilter = _world.Filter<Cell>().Exc<Empty>().End();
            _hexFilter = _world.Filter<Hex>().End();
            _hexPool = _world.GetPool<Hex>();
            _cellPool = _world.GetPool<Cell>();
            _collapseRequestPool = _world.GetPool<CollapseRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _cellFilter)
            {
                if(_gameFlowService.IsAnyoneActing) return;
                
                var count = _hexService.GetTopHexColorCount(e);
                if (count >= 10)
                {
                    foreach (var h in _hexFilter)
                    {
                        var hex = _hexPool.Get(h);
                        if (hex.Target.Id == e)
                        {
                            var cell = _cellPool.Get(e);
                            ref var c = ref _collapseRequestPool.Add(h);
                            c.Delay = cell.Count - 1 - hex.Index;
                        }
                    }
                }
            }
        }
    }
}