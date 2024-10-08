using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<DragStarted> _dragStartedPool;
        private EcsPool<Drag> _dragPool;
        private EcsPool<DragEnded> _dragEndedPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _dragStartedPool = _world.GetPool<DragStarted>();
            _dragPool = _world.GetPool<Drag>();
            _dragEndedPool = _world.GetPool<DragEnded>();
        }

        public void Run(IEcsSystems systems)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ref DragStarted started = ref _dragStartedPool.Send();
                started.MousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                ref Drag dragging = ref _dragPool.Send();
                dragging.MousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _dragEndedPool.Send();
            }
        }
    }
}