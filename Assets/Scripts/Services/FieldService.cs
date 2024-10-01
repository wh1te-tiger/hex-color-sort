using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class FieldService
    {
        private readonly Dictionary<Coordinates, int> _cells = new ();
        private readonly Coordinates[] _directions =
        {
            new(1, 0),
            new(-1, 0),
            new(0,1),
            new(0,-1),
            new(1,-1),
            new(-1,1)
        };
        
        public void RegisterCell(int entity, Coordinates pos)
        {
            _cells.Add(pos, entity);
        }
        
        public bool IsCellExists(Coordinates pos)
        {
            return _cells.ContainsKey(pos);
        }

        public int GetCellEntity(Coordinates pos)
        {
            return _cells.GetValueOrDefault(pos, -1);
        }
        
        public bool TryGetNeighbors(Coordinates pos, out Coordinates[] neighbors)
        {
            neighbors = null;
            if (!IsCellExists(pos)) return false;
            
            neighbors = _directions.Select(d => pos + d).Where(IsCellExists).ToArray();
            return true;
        }
    }
}