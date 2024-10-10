using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public struct MoveTween : ITweenComponent
    {
        public Transform Transform;
        public Vector3? From;
        public Vector3 To;
        public bool IsWorld;

        public bool Handle(float t)
        {
            if (Transform == null)
            {
                //trow target null
                return false;
            }

            var from = From ?? Transform.position;

            var pos = Vector3.LerpUnclamped(from, To, t);
            if (IsWorld)
                Transform.position = pos;
            else
                Transform.localPosition = pos;

            return true;
        }
    }
    
    public static class TweenMoveExtensions
    {
        public static ref TweenSettings DoMove(this Transform transform, EcsWorld world, Vector3 to, float time, bool isWorld = true)
        {
            var tweenEntity = world.NewEntity();
            ref var tweenSettings = ref world.GetPool<TweenSettings>().Add(tweenEntity);
            ref var tween = ref world.GetPool<MoveTween>().Add(tweenEntity);

            tween.Transform = transform;
            tween.To = to;
            tween.IsWorld = isWorld;
            
            tweenSettings.Duration = time;
            
            return ref tweenSettings;
        }
    }
}