using Leopotam.EcsLite;

namespace Scripts
{
    public class CheckCollapseStateSystem : IEcsInitSystem , IEcsRunSystem
    {
        private readonly HexService _hexService;
        private readonly ProcessService _processService;
        
        private EcsWorld _world;
        
        private EcsFilter _cellFilter;
        private EcsFilter _hexFilter;
        
        private EcsPool<Hex> _hexPool;
        private EcsPool<Cell> _cellPool;
        private EcsPool<CollapseRequest> _collapseRequestPool;
        private EcsPool<TopHex> _topHexPool;

        public CheckCollapseStateSystem(ProcessService processService, HexService hexService)
        {
            _hexService = hexService;
            _processService = processService;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cellFilter = _world.Filter<Cell>().Exc<Empty>().End();
            _hexFilter = _world.Filter<Hex>().Inc<Active>().End();
            _hexPool = _world.GetPool<Hex>();
            _topHexPool = _world.GetPool<TopHex>();
            _cellPool = _world.GetPool<Cell>();
            _collapseRequestPool = _world.GetPool<CollapseRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            if(_processService.IsAnyProcess) return;
            
            foreach (var e in _cellFilter)
            {
                var count = _hexService.GetTopColorHexCount(e);
                //TODO: move to settings
                if (count < 10) continue;
                
                foreach (var h in _hexFilter)
                {
                    var hex = _hexPool.Get(h);
                    if (hex.Target.Id != e) continue;
                    
                    var cell = _cellPool.Get(e);
                    if ( hex.Index >= cell.Count - count)
                    {
                        ref var c = ref _collapseRequestPool.Add(h);
                        c.Delay = cell.Count - 1 - hex.Index;
                    }

                    if (hex.Index == cell.Count - count - 1)
                    {
                        _topHexPool.Add(h);
                    }
                }
            }
        }
    }
}