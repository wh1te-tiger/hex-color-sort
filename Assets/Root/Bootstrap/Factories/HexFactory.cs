using UnityEngine;
using Leopotam.EcsLite;

namespace Root
{
    public class HexFactory : EntityProviderFactory<HexViewModel>
    {
        private readonly LevelSettings _settings;
        
        public HexFactory(EcsWorld world, LevelSettings settings) : base(world)
        {
            _settings = settings;
        }

        protected override HexViewModel CreateFunc()
        {
            var container = Object.Instantiate(_settings.HexPrefab).GetComponent<HexViewModel>();
            container.GetComponent<EntityConverter>().Convert(World);
            return container;
        }
    }
}