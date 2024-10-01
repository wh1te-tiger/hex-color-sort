using System;
using Scripts;

namespace Data
{
    [Serializable]
    public struct CellData
    {
        public Coordinates coordinates;
        public bool isLocked;
        public HexData[] hexes;
    }
    
    
}