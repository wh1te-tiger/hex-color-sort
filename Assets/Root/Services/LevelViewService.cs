using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Root
{
    public class LevelViewService : IInitializable
    {
        #region Dependencies
        
        private readonly EcsWorld _world;
        
        #endregion
        
        #region State
        
        private LevelViewModel _level;
        
        #endregion
        
        public LevelViewService(EcsWorld world)
        {
            _world = world;
        }
        
        public void Initialize()
        {
            _level = Object.FindObjectOfType<LevelViewModel>();
            foreach (var entityConverter in _level.SlotsRoot.GetComponentsInChildren<EntityConverter>())
            {
                entityConverter.Convert(_world);
            }
        }
    }
}