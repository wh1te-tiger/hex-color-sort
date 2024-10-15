using Leopotam.EcsLite;

namespace Scripts
{
    public struct Cell
    {
        public Coordinates FieldPosition;
        public int Count;
        public EcsPackedEntity[] Neighbors;
        public ColorId TopHexColor;
    }
}