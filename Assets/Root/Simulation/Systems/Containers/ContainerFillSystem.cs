using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class ContainerFillSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly HexFactory _hexFactory;
        private readonly ColorSettings _colorSettings;
        
        private EcsFilter _initRequestFilter;
        private EcsPool<Color> _colorPool;
        private EcsPool<Parent> _parentPool;
        private EcsPool<ChildRoot> _childRootPool;
        private EcsPool<Hexes> _hexesPool;
        private EcsPool<Source> _sourcePool;

        public ContainerFillSystem(HexFactory hexFactory, ColorSettings colorSettings)
        {
            _hexFactory = hexFactory;
            _colorSettings = colorSettings;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().Inc<Hexes>().End();
            
            _colorPool = world.GetPool<Color>();
            _parentPool = world.GetPool<Parent>();
            _childRootPool = world.GetPool<ChildRoot>();
            _hexesPool = world.GetPool<Hexes>();
            _sourcePool = world.GetPool<Source>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var e in _initRequestFilter)
            {
                SpawnHexes(e);
            }
        }

        private void SpawnHexes(int parent)
        {
            var hexesCount = Random.Range(3, 8);
            var colorsCount = _colorSettings.GetColorsCount(hexesCount);
            var e = Enumerable
                .Range(0, colorsCount)
                .Select(x => Random.Range(0, 1f))
                .ToArray();
            var sum = e.Sum();
            var hexesByColorCount = e
                .Select(( v,i ) => new { Count = Mathf.RoundToInt(v / sum * hexesCount), Index = i} )
                .Join(_colorSettings.GetRandomColors(colorsCount)
                        .Select((c, i) => new {ColorName = c, Index = i}), count => count.Index, color => color.Index,
                    (count, color) => new { color.ColorName, count.Count });

            ref var hexesComponent = ref _hexesPool.Get(parent);
            ref var childRootComponent = ref _childRootPool.Get(parent);
            
            foreach (var record in hexesByColorCount)
            {
                for (var i = 0; i < record.Count; i++)
                {
                    //создали
                    var hex = _hexFactory.Create();
                    
                    //разукрасили
                    ref var colorComponent = ref _colorPool.Get(hex);
                    colorComponent.Property.Value = _colorSettings.Get(record.ColorName);
                    colorComponent.Id = record.ColorName;
                    
                    //добавили хекс родителю 
                    hexesComponent.Value.Add(hex);
                    
                    //установили родителя вьюхи хекса
                    ref var parentComponent = ref _parentPool.Get(hex);
                    parentComponent.Property.SetValueAndForceNotify(childRootComponent.Value);
                }
            }
        }
    }
}