using System;
using System.Linq;
using Leopotam.EcsLite;
using UniRx;
using Zenject;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Root
{
    public class LevelViewService : IInitializable
    {
        #region Dependencies
        
        private readonly EcsWorld _world;
        private readonly FieldFactory _fieldFactory;
        
        #endregion
        
        #region State

        public Transform SlotSpawnPos => _level.SlotsSpawnPos;
        public Transform HexContainer => _level.HexContainer;
        
        private LevelViewModel _level;
        private readonly EcsPool<EmptySlotsEvent> _pool;
        
        #endregion
        
        public LevelViewService(EcsWorld world, FieldFactory fieldFactory)
        {
            _world = world;
            _fieldFactory = fieldFactory;
            _pool = _world.GetPool<EmptySlotsEvent>();
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

            var field = _fieldFactory.Create();
            field.transform.parent = _level.FieldRoot;
            field.transform.localPosition = new Vector3();
        }

        public Transform GetFirstFreeSlotTransform()
        {
            var res = _level.SlotsRoot
                .Cast<Transform>()
                .FirstOrDefault(t => t.childCount == 0);

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