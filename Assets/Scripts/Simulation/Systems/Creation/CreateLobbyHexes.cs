using System;
using Leopotam.EcsLite;

namespace Scripts
{
    public class CreateLobbyHexes : IEcsInitSystem
    {
        private readonly HexFactory _factory;
        private readonly FieldSettings _fieldSettings;
        private readonly FieldService _fieldService;

        public CreateLobbyHexes(FieldService fieldService,HexFactory factory, FieldSettings fieldSettings)
        {
            _factory = factory;
            _fieldSettings = fieldSettings;
            _fieldService = fieldService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var hexPool = world.GetPool<Hex>();
            var unorderedPool = world.GetPool<Unordered>();
            var modelCreated = world.GetPool<ModelCreated>();
            var activePool = world.GetPool<Active>();
            
            foreach (var cellData in _fieldSettings.cells)
            {
                var cellEntity = _fieldService.GetCellEntity(cellData.coordinates);
                
                var index = 0;
                foreach (var hexData in cellData.hexes)
                {
                    for (var i = 0; i < hexData.count; i++)
                    {
                        var provider = _factory.Create();
                        if (!provider.TryGetEntity(out var e)) throw new Exception("Provider without entity");

                        ref var hex = ref hexPool.GetOrAdd(e);
                        hex.Color = hexData.colorId;
                        hex.Target = world.PackEntity(cellEntity);
                        hex.Index = index;
                        
                        modelCreated.Add(e);
                        activePool.Add(e);
                        unorderedPool.Add(e);
                        
                        ref var targetChanged = ref world.Send<TargetChanged>();
                        targetChanged.New = hex.Target;
                        
                        index++;
                    }
                }
            }
        }
    }
}