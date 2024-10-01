using Leopotam.EcsLite;

namespace Scripts
{
    public class ShiftExecuteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService; 
        
        private EcsWorld _world;
        
        private EcsFilter _shiftRequestFilter;
        private EcsFilter _hexFilter;
        
        private EcsPool<ShiftRequest> _shiftRequestPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<ShiftProcess> _shiftingPool;
        private EcsPool<Cell> _cellPool;

        public ShiftExecuteSystem(GameFlowService gameFlowService)
        {
            _gameFlowService = gameFlowService;
        }
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _shiftRequestFilter = _world.Filter<ShiftRequest>().End();
            _hexFilter = _world.Filter<Hex>().End();

            _cellPool = _world.GetPool<Cell>();
            _hexPool = _world.GetPool<Hex>();
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
            foreach (var h in _hexFilter)
            {
                ref var hex = ref _hexPool.Get(h);
                if ( !hex.Target.Id.Equals(from) ) continue;

                if (hex.Index >= fromCount - shiftCount)
                {
                    var newIndex = fromCount - hex.Index + toCount - 1;
                    
                    hex.Target = _world.PackEntity(to);
                    hex.Index = newIndex;
                    
                    //TODO: replace with settings;
                    var delay = (newIndex - toCount) * 0.1f;
                    StartShiftProcess(h, to, newIndex + 1, delay);

                    ref var targetChanged = ref _world.Send<TargetChanged>();
                    targetChanged.Old = _world.PackEntity(from);
                    targetChanged.New = hex.Target;
                }
            }
        }

        private void StartShiftProcess(int e, int target, int height, float delay)
        {
            ref var shiftProcess = ref _gameFlowService.StartNewProcess(_shiftingPool, e);
            shiftProcess.Target = _world.PackEntity(target);
            shiftProcess.Height = height;
            shiftProcess.Delay = delay;
        }
    }
}