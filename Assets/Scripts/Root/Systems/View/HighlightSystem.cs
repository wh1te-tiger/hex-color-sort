using Leopotam.EcsLite;

namespace Scripts
{
    public class HighlightSystem : IEcsInitSystem, IEcsRunSystem
    {
        private ViewSettings _viewSettings;

        private EcsWorld _world;
        
        private EcsFilter _cellFilter;
        private EcsFilter _highlightRequestFilter;
        private EcsFilter _highlightedFilter;

        private EcsPool<Cell> _cellPool;
        private EcsPool<HighlightRequest> _highlightRequestPool;
        private EcsPool<Highlighted> _highlightedPool;
        private EcsPool<MonoLink<Colorable>> _colorablePool;
        

        public HighlightSystem(ViewSettings viewSettings)
        {
            _viewSettings = viewSettings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _cellFilter = _world.Filter<Cell>().End();
            _highlightRequestFilter = _world.Filter<HighlightRequest>().End();
            _highlightedFilter = _world.Filter<Highlighted>().Inc<MonoLink<Colorable>>().End();

            _cellPool = _world.GetPool<Cell>();
            _highlightRequestPool = _world.GetPool<HighlightRequest>();
            _highlightedPool = _world.GetPool<Highlighted>();
            _colorablePool = _world.GetPool<MonoLink<Colorable>>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var r in _highlightRequestFilter)
            {
                var request = _highlightRequestPool.Get(r);

                if (request.Target == -1)
                {
                    ResetHighlighted();
                    continue;
                }
                
                if (_highlightedPool.Has(request.Target))
                {
                    _highlightRequestPool.Del(r);
                    continue;
                }
                
                ResetHighlighted();
                HighlightCell(request.Target);
                
                _highlightRequestPool.Del(r);
            }
        }

        private void ResetHighlighted()
        {
            foreach (var h in _highlightedFilter)
            {
                var highlighted = _highlightedPool.Get(h);
                var colorable = _colorablePool.Get(highlighted.Target);
                colorable.Value.Color = _viewSettings.BaseCellColor;
                _highlightedPool.Del(h);
            }
        }

        private void HighlightCell(int cell)
        {
            var colorable = _colorablePool.Get(cell);
            colorable.Value.Color = _viewSettings.HighlightedCellColor;
            ref var highlighted = ref _highlightedPool.Add(cell);
            highlighted.Target = cell;
        }
    }
}