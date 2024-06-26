using Leopotam.EcsLite;

namespace Root
{
    public interface IEntityListener
    {
        void Inject(EcsWorld world, int entity);
    }
}