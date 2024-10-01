using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public static class EcsProviderExtensions
    {
        public static void CreatViewForEntity(this EcsWorld world, int entity, GameObject prefab,
            Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject obj;
            /*if (pool != null)
                obj = pool.Spawn(prefab, position, rotation, parent);
            else
                obj = Object.Instantiate(prefab, position, rotation, parent);*/
            
            obj = Object.Instantiate(prefab, position, rotation, parent);
            var provider = obj.GetComponent<EntityProvider>();
            provider.Inject(world, entity);
        }
    }
}