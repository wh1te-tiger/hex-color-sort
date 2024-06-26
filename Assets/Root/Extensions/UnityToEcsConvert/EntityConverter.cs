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

            var components = GetComponents<EntityProvider>();
            foreach (var component in components)
            {
                component.Inject(world, entity);
            }
            
            _packedEntityWithWorld = world.PackEntityWithWorld(entity);
            _converted = true;
        }
        
        private void OnDestroy()
        {
            if (_converted && destroyEntityWithGameObject && _packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                world.DelEntity(entity);
                _packedEntityWithWorld = default;
            }
        }
        
        public EcsPackedEntityWithWorld GetEntity()
        {
            return _packedEntityWithWorld;
        }
    }
}