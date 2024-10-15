using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    [DisallowMultipleComponent]
    public class EntityProvider : MonoBehaviour
    {
        [field: SerializeField] private int ID { get; set; }

        #region State

        private EcsPackedEntity _entity;
        private EcsWorld _world;
        private bool _isInitialized;

        #endregion
        
        public bool Inject(EcsWorld world, int entity)
        {
            if(_isInitialized) return false;
            
            _entity = world.PackEntity(entity);
            _world = world;

            ConvertComponents();
            
            _isInitialized = true;
            ID = _entity.Id;
            return true;
        }

        public void Inject(EcsWorld world)
        {
            if(_isInitialized) return;
            
            _world = world;
            var e = _world.NewEntity();
            
            _entity = world.PackEntity(e);

            ConvertComponents();
            ID = _entity.Id;
            
            _isInitialized = true;
        }
        
        public bool TryGetEntity(out int entity)
        {
            entity = -1;
            return _world != null && _entity.Unpack(_world, out entity);
        }
        
        public ref T Get<T>() where T : struct
        {
            return ref _world.GetPool<T>().Get(_entity.Id);
        }
        
        public bool Has<T>() where T : struct
        {
            return _world.GetPool<T>().Has(_entity.Id);
        }

        private void ConvertComponents()
        {
            foreach (var convertable in GetComponents<IConvertableToEntity>())
            {
                convertable.ConvertToEntity(_world, _entity.Id);
            }
        }
    }
}