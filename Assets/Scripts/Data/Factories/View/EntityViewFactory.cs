using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public abstract class EntityViewFactory
    {
        protected readonly EcsWorld World;

        internal EntityViewFactory(EcsWorld world)
        {
            World = world;
        }

        public abstract GameObject Create(int entity);

        protected EntityProvider CreateView(GameObject prefab, Transform root)
        {
            var obj = Object.Instantiate(prefab, root);
            var provider = obj.GetComponent<EntityProvider>();
            return provider;
        }
    }
}