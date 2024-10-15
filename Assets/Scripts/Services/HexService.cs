using System.Linq;
using Leopotam.EcsLite;

namespace Scripts
{
    public class HexService
    {
        #region State

        private readonly EcsFilter _filter;
        private readonly EcsPool<Hex> _pool;
        private int HexTotalCount => _filter.GetEntitiesCount();

        #endregion

        public HexService(EcsWorld world)
        {
            _filter = world.Filter<Hex>().Inc<Active>().End();
            _pool = world.GetPool<Hex>();
        }
        
        //TODO: nu hz
        public int GetTopColorHexCount(int cell)
        {
            var hexesOnCell = _filter
                .GetRawEntities()
                .Take(HexTotalCount)
                .Select(e => _pool.Get(e))
                .Where(hex => hex.Target.Id.Equals(cell))
                .OrderByDescending(h=> h.Index)
                .ToArray();

            var allHexes = _filter
                .GetRawEntities()
                .Take(HexTotalCount)
                .Select(e => _pool.Get(e))
                .ToArray();
            var onCell = allHexes.Where(hex => hex.Target.Id.Equals(cell)).ToArray();
            
            if (hexesOnCell.Length == 0) return 0;
            
            var color = hexesOnCell[0].Color;
            return hexesOnCell
                .TakeWhile(h => h.Color == color)
                .Count();
        }
    }
}