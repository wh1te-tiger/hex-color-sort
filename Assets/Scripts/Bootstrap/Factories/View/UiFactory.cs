using UnityEngine;
using Zenject;

namespace Scripts
{
    public class UiFactory : PlaceholderFactory<GameObject, object[], Transform, WindowPresenter>
    {
        private readonly DiContainer _container;

        public UiFactory(DiContainer container)
        {
            _container = container;
        }

        public override WindowPresenter Create(GameObject prefab, object[] parameters, Transform root)
        {
            var presenter = _container.InstantiatePrefabForComponent<WindowPresenter>(prefab, root, parameters);
            presenter.Enabled = false;
            if (presenter is IInitializable initialize) initialize.Initialize(); 
            return presenter;
        }
    }
}