using UnityEngine;
using Leopotam.EcsLite;

namespace Root
{
    public class HexFactory : EntityProviderFactory<HexViewModel>
    {
        private readonly CoreSettings _settings;
        
        public HexFactory(EcsWorld world, CoreSettings settings) : base(world)
        {
            _settings = settings;
        }

        protected override HexViewModel CreateFunc()
        {
            var container = Object.Instantiate(_settings.HexPrefab).GetComponent<HexViewModel>();
            var entity = World.NewEntity();
            Dictionary.Add(container, entity);
            container.GetComponent<EntityConverter>().Convert(World, entity);
            return container;
        }
    }
}