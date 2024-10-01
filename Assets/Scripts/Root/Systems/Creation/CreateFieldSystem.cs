using Leopotam.EcsLite;
using Scripts;

namespace Creation
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