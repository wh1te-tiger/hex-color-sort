using UnityEngine;
using Zenject;

namespace Scripts
{
    public class UiFactory : PlaceholderFactory<GameObject, object[], WindowPresenter>
    {
        private readonly DiContainer _container;
        private readonly Transform _root;

        public UiFactory(DiContainer container, Canvas root)
        {
            _container = container;
            _root = root.transform;
        }

        public override WindowPresenter Create(GameObject prefab, object[] parameters)
        {
            var presenter = _container.InstantiatePrefabForComponent<WindowPresenter>(prefab, _root, parameters);
            presenter.Enabled = false;
            if (presenter is IInitializable initialize) initialize.Initialize(); 
            return presenter;
        }
    }
}