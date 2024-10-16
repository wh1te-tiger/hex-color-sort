using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class AlignCameraViewToFieldSystem : IEcsInitSystem
    {
        private const float SpacingFactor = 1.2f;
        
        public void Init(IEcsSystems systems)
        {
            var camera = Camera.main;
            var world = systems.GetWorld();
            var cellFilter = world.Filter<Cell>().Inc<MonoLink<Renderer>>().End();
            var rendererPool = world.GetPool<MonoLink<Renderer>>();

            var minX = float.MaxValue;
            var maxX = float.MinValue;
            
            foreach (var e in cellFilter)
            {
                var renderer = rendererPool.Get(e).Value;
                var bounds = renderer.bounds;
                minX = Mathf.Min(bounds.min.x, minX);
                maxX = Mathf.Max(bounds.max.x, maxX);
            }
            
            var orthographicSize = Mathf.Abs((maxX - minX) * SpacingFactor) / camera.aspect / 2f;
            camera.orthographicSize = Mathf.Max(orthographicSize, .01f);
        }
    }
}