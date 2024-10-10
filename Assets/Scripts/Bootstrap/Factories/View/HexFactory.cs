using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class HexFactory : FactoryWithPool<EntityProvider>
    {
        private readonly ViewSettings _viewSettings;
        private readonly Transform _root;
        
        private readonly EcsWorld _world;

        public HexFactory(EcsWorld world, ViewSettings viewSettings, Transform root)
        {
            _world = world;
            _viewSettings = viewSettings;
            _root = root;
        }
        
        protected override EntityProvider CreateFunction()
        {
            var obj = Object.Instantiate(_viewSettings.HexPrefab, _root);
            var provider = obj.GetComponent<EntityProvider>();
            provider.Inject(_world);
            return provider;
        }

        protected override void OnReleaseFunction(EntityProvider element)
        {
            base.OnReleaseFunction(element);
            element.gameObject.SetActive(false);
            element.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            element.transform.localScale = Vector3.one;
        }

        protected override void OnGetFunction(EntityProvider element)
        {
            base.OnGetFunction(element);
            element.gameObject.SetActive(true);
        }
    }
}