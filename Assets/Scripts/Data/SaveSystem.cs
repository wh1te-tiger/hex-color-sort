using System.Linq;
using Leopotam.EcsLite;

namespace Scripts
{
    public class SaveSystem : UnityEventHandler.IPause, UnityEventHandler.IQuit
    {
        private readonly AppSessionData _data;
        private readonly LevelService _levelService;
        private readonly EcsFilter _hexFilter;
        private readonly EcsPool<Hex> _hexPool;
        private readonly EcsPool<Cell> _cellPool;

        public SaveSystem(AppSessionData data, LevelService levelService, Signal returnToLobbyRequest, EcsWorld world)
        {
            _data = data;
            _levelService = levelService;
            _hexFilter = world.Filter<Hex>().Inc<Active>().End();
            _hexPool = world.GetPool<Hex>();
            _cellPool = world.GetPool<Cell>();
            
            returnToLobbyRequest.Subscribe(TrySave);
        }

        public void Pause(bool pause)
        {
            if (pause)
            {
                TrySave();
            }
        }

        public void Quit()
        {
            TrySave();
        }

        private void TrySave()
        {
            var fieldId = _data.NextLevelID;
            var score = _levelService.Score;
            var hexes = _hexFilter
                    .GetRawEntities()
                    .Take(_hexFilter.GetEntitiesCount())
                    .Select(e => _hexPool.Get(e))
                    .Where(h => _cellPool.Has(h.Target.Id))
                    .Select(h => new HexData(h.Color, h.Index, _cellPool.Get(h.Target.Id).FieldPosition))
                    .ToArray();
            var state = _levelService.LevelState.Value;
            _data.OngoingCoreSession.CoreData = new CoreData(fieldId, score, hexes, state);
        }
    }
}