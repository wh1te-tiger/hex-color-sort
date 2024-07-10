using UnityEngine;
using Leopotam.EcsLite;

namespace Root
{
    public sealed class EntityConverter : MonoBehaviour
    {
        [SerializeField] private bool destroyEntityWithGameObject;
        
        private bool _converted;
        private EcsPackedEntityWithWorld _packedEntityWithWorld;
        
        
        public void Convert(EcsWorld world)
        {
            if (_converted) return;

            var entity = world.NewEntity();
            CreateInternal(world, entity);
            
            _converted = true;
        }
        
        public void Convert(EcsWorld world, int entity)
        {
            if (_converted) return;
            
            CreateInternal(world, entity);
            
            _converted = true;
        }

        private void CreateInternal(EcsWorld world, int entity)
        {
            var components = GetComponents<EntityProvider>();
            foreach (var component in components)
            {
                component.Inject(world, entity);
            }
            
            _packedEntityWithWorld = world.PackEntityWithWorld(entity);
        }
        
        private void OnDestroy()
        {
            if (_converted && destroyEntityWithGameObject && _packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                world.DelEntity(entity);
                _packedEntityWithWorld = default;
            }
        }
    }
}