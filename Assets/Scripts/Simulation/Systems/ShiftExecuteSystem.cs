using Leopotam.EcsLite;

namespace Scripts
{
    public class ShiftExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ProcessService _processService; 
        
        private EcsWorld _world;
        
        private EcsFilter _shiftRequestFilter;
        private EcsFilter _hexFilter;
        
        private EcsPool<ShiftRequest> _shiftRequestPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<TopHex> _topHexPool;
        private EcsPool<ShiftProcess> _shiftingPool;
        private EcsPool<Cell> _cellPool;
        
        public ShiftExecuteSystem(ProcessService processService)
        {
            _processService = processService;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _shiftRequestFilter = _world.Filter<ShiftRequest>().End();
            _hexFilter = _world.Filter<Hex>().Inc<Active>().End();

            _cellPool = _world.GetPool<Cell>();
            _hexPool = _world.GetPool<Hex>();
            _topHexPool = _world.GetPool<TopHex>();
            _shiftRequestPool = _world.GetPool<ShiftRequest>();
            _shiftingPool = _world.GetPool<ShiftProcess>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var r in _shiftRequestFilter)
            {
                var request = _shiftRequestPool.Get(r);

                var from = request.From.Id;
                var to = request.To.Id;
                var fromCount = _cellPool.Get(from).Count;
                var toCount = _cellPool.Get(to).Count;
                
                ShiftHexes(from, to, fromCount, toCount,  request.Count);
                
                _shiftRequestPool.Del(r);
            }
        }

        private void ShiftHexes(int from, int to, int fromCount, int toCount,  int shiftCount)
        {
            var topHexToIndex = toCount + shiftCount - 1;
            var topHexFromIndex = fromCount - shiftCount - 1;
            
            foreach (var h in _hexFilter)
            {
                ref var hex = ref _hexPool.Get(h);
                if ( !hex.Target.Id.Equals(from) ) continue;
                
                if (hex.Index > topHexFromIndex)
                {
                    var newIndex = fromCount - hex.Index + toCount - 1;
                    
                    hex.Target = _world.PackEntity(to);
                    hex.Index = newIndex;

                    if (hex.Index == topHexToIndex)
                    {
                        _topHexPool.Add(h);
                    }
                    
                    //TODO: replace with settings or move delay to view system
                    var delay = (newIndex - toCount) * 0.1f;
                    StartShiftProcess(h, to, newIndex + 1, delay);

                    ref var targetChanged = ref _world.Send<TargetChanged>();
                    targetChanged.Old = _world.PackEntity(from);
                    targetChanged.New = hex.Target;
                }
                else
                {
                    if (hex.Index == topHexFromIndex)
                    {
                        _topHexPool.Add(h);
                    }
                }
            }
        }

        private void StartShiftProcess(int e, int target, int height, float delay)
        {
            ref var shiftProcess = ref _processService.StartNewProcess(_shiftingPool, e, delay);
            shiftProcess.Target = _world.PackEntity(target);
            shiftProcess.Height = height;
        }
    }
}