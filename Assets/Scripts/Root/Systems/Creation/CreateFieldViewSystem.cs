using Leopotam.EcsLite;
using Scripts;
using UnityEngine;

namespace Creation
{
    public class CreateFieldViewSystem : IEcsInitSystem
    {
        private readonly ViewSettings _viewSettings;
        private readonly Transform _root;

        private EcsFilter _cellsFilter;
        private EcsPool<Cell> _cellsPool;

        public CreateFieldViewSystem(ViewSettings viewSettings, Transform root)
        {
            _viewSettings = viewSettings;
            _root = root;
        }

        //  ⎡x⎤            ⎡    3/2         0    ⎤   ⎡q⎤
        //  ⎢ ⎥  =  size × ⎢                     ⎥ × ⎢ ⎥
        //  ⎣y⎦            ⎣ sqrt(3)/2   sqrt(3) ⎦   ⎣r⎦
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _cellsFilter = world.Filter<Cell>().End();
            _cellsPool = world.GetPool<Cell>();
            
            foreach (var e in _cellsFilter)
            {
                var cell = _cellsPool.Get(e);
                var coordinates = cell.FieldPosition;
                
                var x = _viewSettings.CellWidth * 3 / 2f * coordinates.q;
                var z = Mathf.Sqrt(3) / 2 * coordinates.q + Mathf.Sqrt(3) * coordinates.r;
                
                ref var pos = ref world.GetPool<WorldPosition>().Add(e);
                pos.Value = new Vector3(x, 0, z);
                
                world.CreatViewForEntity(e, _viewSettings.CellPrefab, pos.Value, Quaternion.identity, _root);
                ref var colorable = ref world.GetPool<MonoLink<Colorable>>().Get(e);
                colorable.Value.Color = _viewSettings.BaseCellColor;
            }
        }
    }
}