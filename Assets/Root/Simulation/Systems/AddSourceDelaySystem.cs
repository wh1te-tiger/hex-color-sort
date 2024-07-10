using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Root
{
    public class AddSourceDelaySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _delayFilter;
        private EcsPool<Delay> _delayPool;
        private EcsPool<HexShiftRequest> _requestPool;
        
        private const float Delay = .25f;
        
        private readonly EventListener _eventListener = new();
        private readonly Queue<int> _queue = new();
        private int _count;
        private float _delay;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            
            var filter = world.Filter<HexShiftRequest>().End();
            filter.AddEventListener(_eventListener);
            
            _delayPool = world.GetPool<Delay>();
            _requestPool = world.GetPool<HexShiftRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _eventListener.OnAdd)
            {
                ref var delay = ref _delayPool.Add(e); 
                
                delay.Value = _count == 0 ? Delay : _delay;

                var request = _requestPool.Get(e);
                _delay = request.Count * 0.15f + Delay * request.SequenceNumber;
                _count++;
            }
            _eventListener.OnAdd.Clear();
            
            foreach (var e in _eventListener.OnRemove)
            {
                _count--;
            }
        }
    }
}