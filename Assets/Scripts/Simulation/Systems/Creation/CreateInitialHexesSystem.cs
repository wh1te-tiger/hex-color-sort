using System;
using Leopotam.EcsLite;

namespace Scripts
{
    public class CreateInitialHexesSystem : IEcsInitSystem
    {
        private readonly HexFactory _factory;
        private readonly FieldService _fieldService;
        private readonly CoreSessionData _coreData;

        public CreateInitialHexesSystem(FieldService fieldService, HexFactory factory, CoreSessionData coreData)
        {
            _factory = factory;
            _coreData = coreData;
            _fieldService = fieldService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var hexPool = world.GetPool<Hex>();
            var unorderedPool = world.GetPool<Unordered>();
            var modelCreated = world.GetPool<ModelCreated>();

            foreach (var hexData in _coreData.CoreData.hexes)
            {
                var cellEntity = _fieldService.GetCellEntity(hexData.pos);
                if(cellEntity == -1) throw new Exception($"Cell entity for {hexData.pos} is not created");
                
                var provider = _factory.Create();
                if (!provider.TryGetEntity(out var e)) throw new Exception("Provider without entity");

                ref var hex = ref hexPool.GetOrAdd(e);
                hex.Color = hexData.color;
                hex.Target = world.PackEntity(cellEntity);
                hex.Index = hexData.index;
                        
                modelCreated.Add(e);
                        
                unorderedPool.Add(e);
                        
                ref var targetChanged = ref world.Send<TargetChanged>();
                targetChanged.New = hex.Target;
            }
        }
    }
}