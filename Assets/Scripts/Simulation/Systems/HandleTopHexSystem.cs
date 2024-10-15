using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HandleTopHexSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _topHexFilter;
        private EcsPool<Hex> _hexPool;
        private EcsPool<TopHex> _topHexPool;
        private EcsPool<Cell> _cellPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _topHexFilter = world.Filter<Hex>().Inc<TopHex>().End();
            _hexPool = world.GetPool<Hex>();
            _topHexPool = world.GetPool<TopHex>();
            _cellPool = world.GetPool<Cell>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _topHexFilter)
            {
                var hex = _hexPool.Get(e);
                ref var cell = ref _cellPool.Get(hex.Target.Id);
                cell.TopHexColor = hex.Color;
                _topHexPool.Del(e);
            }
        }
    }
}