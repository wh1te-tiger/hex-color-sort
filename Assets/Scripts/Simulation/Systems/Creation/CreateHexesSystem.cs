using System;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class CreateHexesSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly GameFlowService _gameFlowService;
        private readonly ColorSettings _colorSettings;
        private readonly HexViewFactory _factory;

        private EcsWorld _world;
        private EcsFilter _slotsFilter;
        private EcsPool<Hex> _hexesPool;
        private EcsPool<ModelCreated> _modelCreatedPool;
        private EcsPool<Unordered> _unorderedPool;

        public CreateHexesSystem(GameFlowService gameFlowService, ColorSettings colorSettings, HexViewFactory factory)
        {
            _gameFlowService = gameFlowService;
            _colorSettings = colorSettings;
            _factory = factory;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _slotsFilter = _world.Filter<Slot>().Inc<Empty>().End();
            _hexesPool = _world.GetPool<Hex>();
            _modelCreatedPool = _world.GetPool<ModelCreated>();
            _unorderedPool = _world.GetPool<Unordered>();
        }

        public void Run(IEcsSystems systems)
        {
            if ( !_gameFlowService.IsHexCreationNeeded ) return;
            
            foreach (var slot in _slotsFilter)
            {
                SpawnHexes(slot);
            }
        }
        
        private void SpawnHexes(int parent)
        {
            var hexesCount = Random.Range(3, 8);
            var colorsCount = _colorSettings.GetColorsCount(hexesCount);
            var r = Enumerable
                .Range(0, colorsCount)
                .Select(x => Random.Range(0, 1f))
                .ToArray();
            var sum = r.Sum();
            var hexesByColorCount = r
                .Select(( v,i ) => new { Count = Mathf.RoundToInt(v / sum * hexesCount), Index = i} )
                .Join( _colorSettings.GetRandomColors(colorsCount).Select((c, i) => new {ColorName = c, Index = i}),
                    count => count.Index, 
                    color => color.Index,
                    (count, color) => new { color.ColorName, count.Count });
            
            var index = 0;
            
            foreach (var record in hexesByColorCount)
            {
                for (var i = 0; i < record.Count; i++)
                {
                    var provider = _factory.Create();
                    if (!provider.TryGetEntity(out var e)) throw new Exception("Provider without entity");
                    ref var hex = ref _hexesPool.GetOrAdd(e);
                    hex.Color = record.ColorName;
                    hex.Target = _world.PackEntity(parent);
                    hex.Index = index;
                    
                    _modelCreatedPool.Add(e);
                        
                    _unorderedPool.Add(e);
                    
                    ref var targetChanged = ref _world.Send<TargetChanged>();
                    targetChanged.New = hex.Target;
                    index++;
                }
            }
        }
    }
}