using Leopotam.EcsLite;

namespace Root
{
    internal class EventListener : IEcsFilterEventListener
    {
        public readonly Collector OnAdd = new();
        public readonly Collector OnRemove = new();
        
        public void OnEntityAdded(int entity)
        {
            OnAdd.Add(entity);
        }

        public void OnEntityRemoved(int entity)
        {
            OnRemove.Add(entity);
        }
    }
}