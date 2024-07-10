using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class FindReceiverSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _sourceFilter;
        private EcsPool<Source> _sourcePool;
        private EcsPool<Neighbors> _neighborsPool;
        private EcsPool<Hexes> _hexesPool;
        private EcsPool<Color> _colorPool;
        private EcsPool<HexShiftRequest> _hexShiftRequestPool;
        private EcsWorld _world;

        private EventListener _eventListener;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _sourceFilter = _world
                .Filter<Source>()
                .Inc<Neighbors>()
                .Inc<Hexes>()
                .End();
            
            _sourcePool = _world.GetPool<Source>();
            _neighborsPool = _world.GetPool<Neighbors>();
            _hexesPool = _world.GetPool<Hexes>();
            _colorPool = _world.GetPool<Color>();
            _hexShiftRequestPool = _world.GetPool<HexShiftRequest>();
        }

        public void Run(IEcsSystems systems)
        {
            var i = 1;
            foreach (var e in _sourceFilter)
            {
                var hexes = _hexesPool.Get(e).Value;
                var colorId = _colorPool.Get(hexes[^1]).Id;
                if (TryGetReceiver(_neighborsPool.Get(e).Value, colorId, out var receivers))
                {
                    ref var request = ref _hexShiftRequestPool.Add(_world.NewEntity());

                    request.SequenceNumber = i;
                    request.Source = e;
                    request.Receiver = receivers[0];
                    
                    if (receivers.Length == 1)
                    {
                        var receiver = receivers[0];
                        
                        var isReceiverMonoColor = _hexesPool.Get(receiver).Value.All(h => _colorPool.Get(h).Id == colorId);
                        var isSourceMonoColor = _hexesPool.Get(e).Value.All(h => _colorPool.Get(h).Id == colorId);

                        if (isSourceMonoColor && !isReceiverMonoColor)
                        {
                            request.Source = receiver;
                            request.Receiver = e;
                        }
                    }
                    
                    if (receivers.Length > 1)
                    {
                        request.Source = receivers[Random.Range(0, receivers.Length)];
                        request.Receiver = e;
                    }
                    
                    var receiverHexes = _hexesPool.Get(request.Source).Value;
                    var count = receiverHexes
                        .AsEnumerable()
                        .Reverse()
                        .TakeWhile(hex => _colorPool.Get(hex).Id == colorId)
                        .Count();
                    
                    request.Count = count;
                    
                    i++;
                }
                _sourcePool.Del(e);
            }
        }

        private bool TryGetReceiver(int[] neighbors, ColorName color, out int[] receivers)
        {
            receivers = neighbors
                .Select(n => new { ID = n, Hexes = _hexesPool.Get(n).Value })
                .Where(r => r.Hexes.Count != 0)
                .Where(r => _colorPool.Get(r.Hexes[^1]).Id == color)
                .Select(r => r.ID)
                .ToArray();

            return receivers.Length != 0;
        }
    }
}