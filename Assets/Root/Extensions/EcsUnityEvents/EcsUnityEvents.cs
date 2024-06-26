using UnityEngine;
using UnityEngine.EventSystems;
using Leopotam.EcsLite;

namespace Root
{
    public static class EcsUnityEvents
    {
        public static EcsWorld EcsWorld;

        public static void RegisterBeginDragEvent(GameObject sender, PointerEventData pointerEventData)
        {
            var eventEntity = EcsWorld.NewEntity();
            var pool = EcsWorld.GetPool<BeginDragEvent>();
            pool.Add(eventEntity);
            ref var component = ref pool.Get(eventEntity);
            component.Sender = sender;
            component.PointerEventData = pointerEventData;
        }
        
        public static void RegisterEndDragEvent(GameObject sender, PointerEventData pointerEventData)
        {
            var eventEntity = EcsWorld.NewEntity();
            var pool = EcsWorld.GetPool<EndDragEvent>();
            pool.Add(eventEntity);
            ref var component = ref pool.Get(eventEntity);
            component.Sender = sender;
            component.PointerEventData = pointerEventData;
        }
    }
}