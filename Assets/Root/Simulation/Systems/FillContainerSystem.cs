using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Root
{
    public class FillContainerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly HexFactory _hexFactory;
        private readonly ColorSettings _colorSettings;
        
        private EcsFilter _initRequestFilter;
        private EcsPool<Color> _colorPool;
        private EcsPool<Hexes> _hexesPool;

        public FillContainerSystem(HexFactory hexFactory, ColorSettings colorSettings)
        {
            _hexFactory = hexFactory;
            _colorSettings = colorSettings;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _initRequestFilter = world.Filter<InitRequest>().Inc<Hexes>().End();
            
            _colorPool = world.GetPool<Color>();
            _hexesPool = world.GetPool<Hexes>();
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

            ref var parentHexesComponent = ref _hexesPool.Get(parent);
            
            foreach (var record in hexesByColorCount)
            {
                for (var i = 0; i < record.Count; i++)
                {
                    var hexView = _hexFactory.Create();
                    var entityId = hexView.EntityId;

                    ref var colorComponent = ref _colorPool.Get(entityId);
                    colorComponent.Property.Value = _colorSettings.Get(record.ColorName);
                    colorComponent.Id = record.ColorName;
                    
                    parentHexesComponent.Value.Add(entityId);
                }
            }
        }
    }
}