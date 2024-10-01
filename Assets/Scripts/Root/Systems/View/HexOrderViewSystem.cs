using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HexOrderViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _unorderedFilter;
        private EcsPool<Unordered> _unorderedPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<MonoLink<Transform>> _transformPool;
        private EcsPool<HeightOffset> _heightOffsetPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _unorderedFilter = _world.Filter<Unordered>().Inc<Hex>().Inc<MonoLink<Transform>>().End();
            _unorderedPool = _world.GetPool<Unordered>();
            _hexPool = _world.GetPool<Hex>();
            _transformPool = _world.GetPool<MonoLink<Transform>>();
            _heightOffsetPool = _world.GetPool<HeightOffset>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var e in _unorderedFilter)
            {
                var hex = _hexPool.Get(e);
                
                float offset = 0;
                var target = hex.Target.Id;
                if (_heightOffsetPool.Has(target))
                {
                    offset = _heightOffsetPool.Get(target).Value;
                }
                
                var transform = _transformPool.Get(e).Value;
                transform.position += Vector3.up * (hex.Index * 0.3f + offset);
                _unorderedPool.Del(e);
            }
        }
    }
}