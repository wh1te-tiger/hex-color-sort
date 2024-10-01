using Leopotam.EcsLite;

namespace Scripts
{
    public struct ShiftRequest
    {
        public EcsPackedEntity From;
        public EcsPackedEntity To;
        public int Count;
    }
}