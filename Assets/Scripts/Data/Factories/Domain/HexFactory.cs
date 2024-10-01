using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HexFactory
    {
        private readonly EcsWorld _world;
        private readonly ViewSettings _viewSettings;
        private readonly ColorSettings _colorSettings;

        public HexFactory(EcsWorld world, ViewSettings viewSettings, ColorSettings colorSettings)
        {
            _world = world;
            _viewSettings = viewSettings;
            _colorSettings = colorSettings;
        }

        public int Create(EcsPackedEntity target, ColorId color)
        {
            var e = _world.NewEntity();

            ref var hex = ref _world.GetPool<Hex>().Add(e);
            hex.Color = color;
            hex.Target = target;
            
            _world.CreatViewForEntity(e, _viewSettings.HexPrefab, new Vector3(), Quaternion.identity);
            ref var colorable = ref _world.GetPool<MonoLink<Colorable>>().Get(e);
            colorable.Value.Color = _colorSettings.Get(color);

            return e;
        }
    }
}