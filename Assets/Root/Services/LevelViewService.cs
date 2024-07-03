using System;
using System.Linq;
using Leopotam.EcsLite;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Root
{
    public class LevelViewService : IInitializable
    {
        #region Dependencies
        
        private readonly EcsWorld _world;
        private readonly LevelSettings _levelSettings;
        private readonly ContainerFactory _containerFactory;
        
        #endregion
        
        #region State

        public Transform SlotSpawnPos => _level.SlotsSpawnPos;
        
        private LevelViewModel _level;
        private readonly EcsPool<EmptySlotsEvent> _pool;
        
        #endregion
        
        public LevelViewService(EcsWorld world, LevelSettings levelSettings, ContainerFactory containerFactory)
        {
            _world = world;
            _pool = _world.GetPool<EmptySlotsEvent>();
            _levelSettings = levelSettings;
            _containerFactory = containerFactory;
        }
        
        public void Initialize()
        {
            _level = Object.FindObjectOfType<LevelViewModel>();
            
            _level.SlotsRoot
                .GetComponentsInChildren<Transform>()
                .Skip(1)
                .Select(t => t.ObserveEveryValueChanged(v => v.childCount == 0))
                .CombineLatest()
                .Select(values => values.All(v => v))
                .Where(v => v)
                .AsUnitObservable()
                .Subscribe(_ => HandleEmptySlots())
                .AddTo(_level);
            //HandleEmptySlots();
        }

        public Transform GetFirstFreeSlotTransform()
        {
            Transform res = null;
            
            foreach (Transform t in _level.SlotsRoot)
            {
                if ( t.childCount == 0 )
                {
                    res = t;
                }
            }

            if (res == null)
            {
                throw new Exception("There is no free slots");
            }
            return res;
        }

        private void HandleEmptySlots()
        {
            var e= _world.NewEntity();
            _pool.Add(e);
        }
    }
}