using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine.Pool;

namespace Root
{
    public abstract class EntityProviderFactory<T> where T : EntityProvider
    {
        protected readonly EcsWorld World;
        protected readonly Dictionary<T, int> Dictionary;
        
        private readonly ObjectPool<T> _pool;

        protected EntityProviderFactory(EcsWorld world)
        {
            World = world;
            _pool = new ObjectPool<T>(CreateFunc, OnGetFunction, OnRemoveFunction);
            Dictionary = new Dictionary<T, int>();
        }
        
        public int Create()
        {
            return Dictionary[_pool.Get()];
        }
        
        public void Release(T element)
        {
            _pool.Release(element);
        }

        protected abstract T CreateFunc();

        protected virtual void OnGetFunction(T element){}
        
        protected virtual void OnRemoveFunction(T element){}
    }
}