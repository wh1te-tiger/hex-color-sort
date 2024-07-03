using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    [RequireComponent(typeof(EntityConverter))]
    public abstract class EntityProvider : MonoBehaviour
    {
        protected EcsWorld World { get; private set; }
        protected int EntityId { get; private set; }
        
        public virtual void Inject(EcsWorld world, int entity)
        {
            World = world;
            EntityId = entity;
            Setup();
        }
        
        protected ref T Add<T>() where T : struct
        {
            return ref World.GetPool<T>().Add(EntityId);
        }
        
        protected ref T Get<T>() where T : struct
        {
            return ref World.GetPool<T>().Get(EntityId);
        }
        
        protected ref T GetOrAdd<T>() where  T : struct
        {
            var pool = World.GetPool<T>();
            if (pool.Has(EntityId))
                return ref pool.Get(EntityId);
            return ref pool.Add(EntityId);
        }
        
        protected bool Has<T>() where T : struct
        {
            return World.GetPool<T>().Has(EntityId);
        }
        
        protected void Del<T>() where T : struct
        {
            World.GetPool<T>().Del(EntityId);
        }

        protected abstract void Setup();
    }
}