using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class ContainerFactory : EntityProviderFactory<ContainerViewModel>
    {
        private readonly LevelSettings _settings;

        public ContainerFactory(EcsWorld world, LevelSettings settings) : base(world)
        {
            _settings = settings;
        }
        
        protected override ContainerViewModel CreateFunc()
        {
            var container = Object.Instantiate(_settings.ContainerPrefab).GetComponent<ContainerViewModel>();
            container.GetComponent<EntityConverter>().Convert(World);
            return container;
        }

        protected override void OnGetFunction(ContainerViewModel container)
        {
            container.AddInitRequest();
        }
    }
}