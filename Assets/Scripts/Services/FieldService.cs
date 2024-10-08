using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        
        //  ⎡q⎤     ⎡   2/3         0    ⎤   ⎡x⎤
        //  ⎢ ⎥  =  ⎢                    ⎥ × ⎢ ⎥ ÷ size
        //  ⎣r⎦     ⎣  -1/3    sqrt(3)/3 ⎦   ⎣y⎦
        
        public static Coordinates GetHexCoordinates(Vector3 pos, float cellWidth)
        {
            var q = (2f / 3 * pos.x) / cellWidth;
            var r = (-1f / 3 * pos.x + Mathf.Sqrt(3) / 3 * pos.z) / cellWidth;
            return RoundCoordinates(new Fractional(q, r));
        }
        
        private static Coordinates RoundCoordinates(Fractional frac)
        {
            var q = Mathf.RoundToInt(frac.q);
            var r = Mathf.RoundToInt(frac.r);
            var s = Mathf.RoundToInt(frac.s);

            var qDiff = Mathf.Abs(q - frac.q);
            var rDiff = Mathf.Abs(r - frac.r);
            var sDiff = Mathf.Abs(s - frac.s);

            if (qDiff > rDiff && qDiff > sDiff)  
                q = -r - s;
            else if (rDiff > sDiff) 
                r = -q - s;
            else 
                s = -q - r;
            return new Coordinates(q, r, s);
        }
    }
}