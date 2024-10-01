using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(EntityProvider))]
    public abstract class MonoConvertable<T> : MonoBehaviour, IConvertableToEntity where T : struct
    {
        protected T Value { get; set; }
        
        public void ConvertToEntity(EcsWorld world, int entity)
        {
            var pool = world.GetPool<T>();
            pool.Add(entity) = Value;
            
            Destroy(this);
        }
    }
}