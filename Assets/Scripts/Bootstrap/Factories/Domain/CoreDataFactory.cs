using System.Linq;

namespace Scripts
{
    public class CoreDataFactory
    {
        private readonly AppSessionData _appData;

        public CoreDataFactory(AppSessionData appData)
        {
            _appData = appData;
        }

        public CoreSessionData Create()
        {
            var level = _appData.GetLevel();

            var hexes = level.Field.cells
                .SelectMany(c => c.hexes
                    .SelectMany(h => Enumerable.Range(0, h.count)
                        .Select(_ => new { Color = h.colorId, Pos = c.coordinates }))
                    .GroupBy(v => v.Pos)
                    .SelectMany(g => g.Select((h, i) => new HexData(h.Color, i, h.Pos))))
                .ToArray();

            return new CoreSessionData(new CoreData(level.Id, 0, hexes));
        }
    }
}