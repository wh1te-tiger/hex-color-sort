using Leopotam.EcsLite;

namespace Scripts
{
    public class CreateInitialHexesSystem : IEcsInitSystem
    {
        private readonly FieldSettings _fieldSettings;
        private readonly FieldService _fieldService;

        public CreateInitialHexesSystem(FieldService fieldService, FieldSettings fieldSettings)
        {
            _fieldSettings = fieldSettings;
            _fieldService = fieldService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var hexPool = world.GetPool<Hex>();
            var unorderedPool = world.GetPool<Unordered>();
            var modelCreated = world.GetPool<ModelCreated>();
            
            foreach (var cellData in _fieldSettings.cells)
            {
                var cellEntity = _fieldService.GetCellEntity(cellData.coordinates);
                
                var index = 0;
                foreach (var hexData in cellData.hexes)
                {
                    for (var i = 0; i < hexData.count; i++)
                    {
                        var e = world.NewEntity();

                        ref var hex = ref hexPool.Add(e);
                        hex.Color = hexData.colorId;
                        hex.Target = world.PackEntity(cellEntity);
                        hex.Index = index;
                        
                        modelCreated.Add(e);
                        
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