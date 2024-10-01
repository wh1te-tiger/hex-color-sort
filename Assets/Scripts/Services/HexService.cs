using System.Linq;
using Leopotam.EcsLite;

namespace Scripts
{
    public class HexService
    {
        private EcsFilter Filter { get; }

        private EcsPool<Hex> Pool { get; }

        private int HexTotalCount => Filter.GetEntitiesCount();

        public HexService(EcsWorld world)
        {
            Filter = world.Filter<Hex>().End();
            Pool = world.GetPool<Hex>();
        }
        
        public ColorId GetTopHexColor(int cell)
        {
            var hexesOnCell = Filter
                .GetRawEntities()
                .Take(HexTotalCount)
                .Select(e => Pool.Get(e))
                .Where(hex => hex.Target.Id.Equals(cell))
                .ToArray();
            
            return hexesOnCell.Length != 0 ? hexesOnCell.Single(h => h.Index == hexesOnCell.Length - 1).Color : ColorId.None;
        }

        public int GetTopHexColorCount(int cell)
        {
            var hexesOnCell = Filter
                .GetRawEntities()
                .Take(HexTotalCount)
                .Select(e => Pool.Get(e))
                .Where(hex => hex.Target.Id.Equals(cell))
                .OrderByDescending(h=> h.Index)
                .ToArray();
            
            if (hexesOnCell.Length == 0) return 0;
            
            var color = hexesOnCell[0].Color;
            return hexesOnCell
                .TakeWhile(h => h.Color == color)
                .Count();
        }
    }
}