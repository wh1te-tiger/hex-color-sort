using Leopotam.EcsLite;

namespace Scripts
{
    public struct MoveProcess : IProcessData
    {
        public EcsPackedEntity Target;
        public float Delay;
    }
}