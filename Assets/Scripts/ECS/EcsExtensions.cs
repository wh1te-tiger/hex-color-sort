using Leopotam.EcsLite;

namespace Scripts
{
    public static class EcsExtensions
    {
        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
                return ref pool.Get(entity);
            
            return ref pool.Add(entity);
        }
        
        public static ref T Send<T>(this EcsWorld world) where T : struct
        {
            var pool = world.GetPool<T>();
            var e = world.NewEntity();
            return ref pool.Add(e);
        }
        
        public static ref T Send<T>(this EcsPool<T> pool) where T : struct
        {
            var world = pool.GetWorld();
            var entity = world.NewEntity();
            return ref pool.Add(entity);
        }
        
        public static ref T Send<T>(this EcsPool<T> pool, out int entity) where T : struct
        {
            var world = pool.GetWorld();
            entity = world.NewEntity();
            return ref pool.Add(entity);
        }
    }
}