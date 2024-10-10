using UnityEngine.Pool;

namespace Scripts
{
    public abstract class FactoryWithPool<T> : IFactory<T> where T : class
    {
        private readonly ObjectPool<T> _objectPool;
        
        protected FactoryWithPool()
        {
            _objectPool = new ObjectPool<T>(CreateFunction, OnGetFunction, OnReleaseFunction);
        }
        
        public T Create()
        {
            return _objectPool.Get();
        }

        public void Release(T obj)
        {
            _objectPool.Release(obj);
        }
        
        protected abstract T CreateFunction();
        
        protected virtual void OnGetFunction(T element){}
        
        protected virtual void OnReleaseFunction(T element){}
    }

    public interface IFactory<T>
    {
        public T Create();
        public void Release(T obj);
    }
}