using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HexViewFactory : EntityViewFactory
    {
        private readonly ViewSettings _viewSettings;
        private readonly Transform _root;

        public HexViewFactory(EcsWorld world, ViewSettings viewSettings, Transform root) : base(world)
        {
            _viewSettings = viewSettings;
            _root = root;
        }

        public override GameObject Create(int entity)
        {
            var provider = CreateView(_viewSettings.HexPrefab, _root);
            provider.Inject(World, entity);
            
            return provider.gameObject;
        }
    }
}