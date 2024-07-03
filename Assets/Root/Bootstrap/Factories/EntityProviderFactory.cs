using Leopotam.EcsLite;
using UnityEngine.Pool;

namespace Root
{
    public abstract class EntityProviderFactory<T> where T : EntityProvider
    {
        protected readonly EcsWorld World;
        private readonly ObjectPool<T> _pool;

        protected EntityProviderFactory(EcsWorld world)
        {
            World = world;
            _pool = new ObjectPool<T>(CreateFunc, OnGetFunction);
        }
        
        public T Create()
        {
            return _pool.Get();
        }
        
        public void Release(T element)
        {
            _pool.Release(element);
        }

        protected abstract T CreateFunc();

        protected virtual void OnGetFunction(T element){}
    }
}