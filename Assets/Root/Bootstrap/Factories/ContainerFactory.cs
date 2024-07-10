using System;
using Leopotam.EcsLite;
using UniRx;
using Object = UnityEngine.Object;

namespace Root
{
    public class ContainerFactory : EntityProviderFactory<ContainerViewModel>, IDisposable
    {
        private readonly CoreSettings _settings;

        private readonly CompositeDisposable _disposables = new();

        public ContainerFactory(EcsWorld world, CoreSettings settings) : base(world)
        {
            _settings = settings;
        }
        
        protected override ContainerViewModel CreateFunc()
        {
            var container = Object.Instantiate(_settings.ContainerPrefab).GetComponent<ContainerViewModel>();
            var entity = World.NewEntity();
            Dictionary.Add(container, entity);
            container.GetComponent<EntityConverter>().Convert(World, entity);
            container
                .ObserveEveryValueChanged(_ => container.gameObject.activeSelf)
                .Where(v => !v)
                .Skip(1)
                .Subscribe(_ =>
                {
                    Release(container);
                })
                .AddTo(_disposables);
            return container;
        }

        protected override void OnGetFunction(ContainerViewModel container)
        {
            container.AddInitRequest();
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}