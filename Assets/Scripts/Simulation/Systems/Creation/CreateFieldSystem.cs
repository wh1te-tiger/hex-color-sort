using Leopotam.EcsLite;
using Scripts;

namespace Scripts
{
    public class CreateFieldSystem : IEcsInitSystem
    {
        private readonly FieldFactory _fieldFactory;

        public CreateFieldSystem(FieldFactory fieldFactory)
        {
            _fieldFactory = fieldFactory;
        }

        public void Init(IEcsSystems systems)
        {
            _fieldFactory.Create();
        }
    }
}