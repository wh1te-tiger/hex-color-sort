using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CreateSlotsSystem : IEcsInitSystem
    {
        private readonly Transform _root;

        public CreateSlotsSystem(Transform root)
        {
            _root = root;
        }

        public void Init(IEcsSystems systems)
        {
            var w = systems.GetWorld();
            foreach (var provider in _root.GetComponentsInChildren<EntityProvider>())
            {
                provider.Inject(w);
                provider.TryGetEntity(out var e);
                w.GetPool<Empty>().Add(e);
            }
        }
    }
}