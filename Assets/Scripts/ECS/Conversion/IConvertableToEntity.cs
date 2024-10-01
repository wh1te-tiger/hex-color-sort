using Leopotam.EcsLite;

namespace Scripts
{
    public interface IConvertableToEntity
    {
        void ConvertToEntity(EcsWorld world, int entity);
    }
}