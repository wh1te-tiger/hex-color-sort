using System;
using System.Linq;
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
            var topHexPool = world.GetPool<TopHex>();
            var activePool = world.GetPool<Active>();

            var hexGroup = _coreData.CoreData.hexes
                .GroupBy(h => h.pos)
                .Select(g => g.OrderBy(h => h.index).ToArray());

            foreach (var hexes in hexGroup)
            {
                var cellEntity = _fieldService.GetCellEntity(hexes[0].pos);
                if (cellEntity == -1) throw new Exception($"Cell entity for {hexes[0].pos} is not created");
                
                for (var i = 0; i < hexes.Length; i++)
                {
                    var hexData = hexes[i];
                    var provider = _factory.Create();
                    if (!provider.TryGetEntity(out var e)) throw new Exception("Provider without entity");

                    ref var hex = ref hexPool.GetOrAdd(e);
                    hex.Color = hexData.color;
                    hex.Target = world.PackEntity(cellEntity);
                    hex.Index = hexData.index;

                    if (i == hexes.Length - 1)
                    {
                        topHexPool.Add(e);
                    }
                    
                    modelCreated.Add(e);
                    unorderedPool.Add(e);
                    activePool.Add(e);
                        
                    ref var targetChanged = ref world.Send<TargetChanged>();
                    targetChanged.New = hex.Target;
                }
            }

            /*foreach (var hexData in hexGroup)
            {
                var cellEntity = _fieldService.GetCellEntity(hexData.pos);
                if (cellEntity == -1) throw new Exception($"Cell entity for {hexData.pos} is not created");
                
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
            }*/
        }
    }
}