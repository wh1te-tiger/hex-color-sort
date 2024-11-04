using System;

namespace Scripts
{
    [Serializable]
    public struct CellData
    {
        public Coordinates coordinates;
        public bool isLocked;
        public Hexes[] hexes;
        public int lockCondition;
    }
}