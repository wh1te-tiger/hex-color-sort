using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CreateHexViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly ColorSettings _colorSettings;

        private EcsFilter _modelCreatedFilter;
        private EcsPool<ModelCreated> _modelCreatedPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<WorldPosition> _worldPositionPool;
        private EcsPool<MonoLink<Colorable>> _colorablePool;
        private EcsPool<MonoLink<Transform>> _transformPool;

        public CreateHexViewSystem(ColorSettings colorSettings)
        {
            _colorSettings = colorSettings;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _modelCreatedFilter = world.Filter<ModelCreated>().Inc<Hex>().End();
            _modelCreatedPool = world.GetPool<ModelCreated>();
            _hexPool = world.GetPool<Hex>();
            _colorablePool = world.GetPool<MonoLink<Colorable>>();
            _transformPool = world.GetPool<MonoLink<Transform>>();
            _worldPositionPool = world.GetPool<WorldPosition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _modelCreatedFilter)
            {
                Transform transform = _transformPool.Get(e).Value;
                
                var hex = _hexPool.Get(e);
                var pos = _worldPositionPool.Get(hex.Target.Id);
                transform.position = pos.Value;
                
                ref var colorable = ref _colorablePool.Get(e);
                colorable.Value.Color = _colorSettings.Get(hex.Color);
                
                _modelCreatedPool.Del(e);
            }
        }
    }
}