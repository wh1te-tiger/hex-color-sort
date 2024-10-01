using Leopotam.EcsLite;

namespace Scripts
{
    public class CreateHexViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly HexViewFactory _viewFactory;
        private readonly ColorSettings _colorSettings;

        private EcsFilter _modelCreatedFilter;
        private EcsPool<ModelCreated> _modelCreatedPool;
        private EcsPool<Hex> _hexPool;
        private EcsPool<WorldPosition> _worldPositionPool;
        private EcsPool<MonoLink<Colorable>> _colorablePool;

        public CreateHexViewSystem(HexViewFactory viewFactory, ColorSettings colorSettings)
        {
            _viewFactory = viewFactory;
            _colorSettings = colorSettings;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _modelCreatedFilter = world.Filter<ModelCreated>().Inc<Hex>().End();
            _modelCreatedPool = world.GetPool<ModelCreated>();
            _hexPool = world.GetPool<Hex>();
            _colorablePool = world.GetPool<MonoLink<Colorable>>();
            _worldPositionPool = world.GetPool<WorldPosition>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _modelCreatedFilter)
            {
                var go = _viewFactory.Create(e);
                
                var hex = _hexPool.Get(e);
                var pos = _worldPositionPool.Get(hex.Target.Id);
                go.transform.position = pos.Value;
                
                ref var colorable = ref _colorablePool.Get(e);
                colorable.Value.Color = _colorSettings.Get(hex.Color);
                
                _modelCreatedPool.Del(e);
            }
        }
    }
}