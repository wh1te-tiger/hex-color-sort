using System;
using Leopotam.EcsLite;

namespace Scripts
{
    internal class EventListener : IEcsFilterEventListener
    {
        public readonly Collector OnAdd = new();
        public readonly Collector OnRemove = new();
        public event Action OnAdded;
        public event Action OnRemoved;
        
        public void OnEntityAdded(int entity)
        {
            if (OnAdd.Add(entity))
            {
                OnAdded?.Invoke();
            }
        }

        public void OnEntityRemoved(int entity)
        {
            if (OnRemove.Add(entity))
            {
                OnRemoved?.Invoke();
            }
        }
    }
}