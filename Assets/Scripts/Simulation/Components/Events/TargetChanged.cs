using Leopotam.EcsLite;

namespace Scripts
{
    public struct TargetChanged
    {
        public EcsPackedEntity Old;
        public EcsPackedEntity New;
    }
}