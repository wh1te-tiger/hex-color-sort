using Leopotam.EcsLite;

namespace Root
{
    public class FinalizeHexShiftsSystem : IEcsInitSystem, IEcsPostRunSystem
    {
        private EcsPool<HexShiftRequest> _hexShiftRequestPool;
        
        private readonly EventListener _eventListener = new();
        private EcsWorld _world;

        private const float LastShiftDelay = .5f;
        private float _timer = 0;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            var filter = _world
                .Filter<HexShiftRequest>()
                .Exc<Delay>()
                .End();
            
            filter.AddEventListener(_eventListener);
        }

        public void PostRun(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnRemove)
            {
                
            }
        }
    }
}