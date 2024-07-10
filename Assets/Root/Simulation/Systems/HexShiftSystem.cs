using Leopotam.EcsLite;

namespace Root
{
    public class HexShiftSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _hexShiftRequestFilter;
        private EcsPool<HexShiftRequest> _hexShiftRequestPool;
        private EcsPool<Hexes> _hexesPool;
        private EcsPool<Delay> _delayPool;
        private EcsPool<Source> _sourcePool;
        private EcsPool<Organized> _organizedPool;
        
        private const float HexDelayMovement = 0.15f;
        
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _hexShiftRequestFilter = world
                .Filter<HexShiftRequest>()
                .Exc<Delay>()
                .End();
            
            _hexShiftRequestPool = world.GetPool<HexShiftRequest>();
            _hexesPool = world.GetPool<Hexes>();
            _delayPool = world.GetPool<Delay>();
            _sourcePool = world.GetPool<Source>();
            _organizedPool = world.GetPool<Organized>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _hexShiftRequestFilter)
            {
                var shiftRequest = _hexShiftRequestPool.Get(e);
                var source = shiftRequest.Source;
                var receiver = shiftRequest.Receiver;
                var count = shiftRequest.Count;
                
                var sourceHexes = _hexesPool.Get(source).Value;
                var receiverHexes = _hexesPool.Get(receiver).Value;
                for (var i = sourceHexes.Count - 1; i >= sourceHexes.Count - count; i--)
                {
                    if (!_delayPool.Has(sourceHexes[i])) _delayPool.Add(sourceHexes[i]);
                    ref var delay = ref _delayPool.Get(sourceHexes[i]);
                    delay.Value = HexDelayMovement * (sourceHexes.Count - 1 - i);
                    receiverHexes.Add(sourceHexes[i]);
                }
                
                if(_organizedPool.Has(receiver)) _organizedPool.Del(receiver);
                _sourcePool.Add(receiver);
                sourceHexes.RemoveRange( sourceHexes.Count - count, count);
                if (sourceHexes.Count != 0)
                {
                    _sourcePool.Add(source);
                }
                
                _hexShiftRequestPool.Del(e);
            }
        }
    }
}