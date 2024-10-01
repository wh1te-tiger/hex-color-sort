using Leopotam.EcsLite;

namespace Scripts
{
    public class FieldFactory
    {
        private readonly EcsWorld _world;
        
        private readonly FieldService _fieldService;
        private readonly FieldSettings _fieldSettings;
        
        public FieldFactory(EcsWorld world, FieldService fieldService, FieldSettings fieldSettings)
        {
            _world = world;
            _fieldService = fieldService;
            _fieldSettings = fieldSettings;
        }

        public void Create()
        {
            foreach (var cellData in _fieldSettings.cells)
            {
                var coordinates = cellData.coordinates;

                var e = _world.NewEntity();
                ref var cell = ref _world.GetPool<Cell>().Add(e);
                cell.FieldPosition = coordinates;
                _world.GetPool<Empty>().Add(e);
                _fieldService.RegisterCell(e, coordinates);
            }
        }
    }
}