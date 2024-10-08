using Leopotam.EcsLite;

namespace Scripts
{
    public class CheckDragOverCellSystem : IEcsInitSystem , IEcsRunSystem
    {
        private readonly DragService _dragService;
        private readonly FieldService _fieldService;
        private readonly GameFlowService _gameFlowService;
        
        private EcsFilter _dragFilter;
        private EcsFilter _selectedFilter;
        
        private EcsPool<Drag> _dragPool;
        private EcsPool<Selected> _selectedPool;
        private EcsPool<Empty> _emptyPool;

        public CheckDragOverCellSystem(GameFlowService gameFlowService, DragService dragService, FieldService fieldService)
        {
            _dragService = dragService;
            _fieldService = fieldService;
            _gameFlowService = gameFlowService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _dragFilter = world.Filter<Drag>().End();
            _selectedFilter = world.Filter<Selected>().Inc<Cell>().End();
            _dragPool = world.GetPool<Drag>();
            _selectedPool = world.GetPool<Selected>();
            _emptyPool = world.GetPool<Empty>();
        }

        public void Run(IEcsSystems systems)
        {
            if(!_gameFlowService.IsDragging) return;
            
            foreach (var e in _dragFilter)
            {
                var drag = _dragPool.Get(e);
                var coordinates = _dragService.ScreenPosToFieldCoordinates(drag.MousePosition);
                var cellEntity = _fieldService.GetCellEntity(coordinates);
                if (cellEntity != -1 && _emptyPool.Has(cellEntity))
                {
                    if (_selectedPool.Has(cellEntity))
                    {
                        continue;
                    }
                    
                    foreach (var s in _selectedFilter)
                    {
                        _selectedPool.Del(s);
                    }
                    _selectedPool.Add(cellEntity);
                }
                else
                {
                    foreach (var s in _selectedFilter)
                    {
                        _selectedPool.Del(s);
                    }
                }
            }
        }
    }
}