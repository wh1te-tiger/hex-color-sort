using Leopotam.EcsLite;

namespace Scripts
{
    public struct ShiftProcess : IProcessData
    {
        public EcsPackedEntity Target;
        public int Height;
    }
}