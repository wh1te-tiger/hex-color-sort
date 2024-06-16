using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Root.Bootstrap
{
    public class GameController : MonoBehaviour
    {
        private EcsWorld _ecsWorld;
        private EcsSystems _systems;
        
        private void Start()
        {
            _ecsWorld = new EcsWorld();
            _systems = new EcsSystems(_ecsWorld);
            
            
        }
        
        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            _ecsWorld?.Destroy();
            _ecsWorld = null;
        }
    }
}