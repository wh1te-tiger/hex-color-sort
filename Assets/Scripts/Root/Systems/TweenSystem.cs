﻿using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public sealed class TweenSystem<TTweenComponent> : IEcsInitSystem, IEcsRunSystem where TTweenComponent : struct, ITweenComponent
    {
        private EcsFilter _filter;
        private EcsPool<TTweenComponent> _tweenPool;
        private EcsPool<TweenSettings> _settingsPool;

        public void Init(IEcsSystems systems)
        {
	        var world = systems.GetWorld();
	        
	        _filter = world.Filter<TTweenComponent>().Inc<TweenSettings>().End();
	        _tweenPool = world.GetPool<TTweenComponent>();
	        _settingsPool = world.GetPool<TweenSettings>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var tweenEntity in _filter)
            {
                var world = systems.GetWorld();
                
                ref TTweenComponent tween         = ref _tweenPool.Get(tweenEntity);
                ref TweenSettings   tweenSettings = ref _settingsPool.Get(tweenEntity);

                if (tweenSettings.IsPause)
                    continue;

                ref var duration = ref tweenSettings.CycleDuration;
                ref var delay = ref tweenSettings.Delay;
                ref var isComplete = ref tweenSettings.IsComplete;
                
                if (isComplete)
                {
                    world.DelEntity(tweenEntity);
                    continue;
                }

                var delta =  Time.deltaTime;

                if ((delay -= delta) > 0.0f)
                    continue;
                
                duration += delta / tweenSettings.Duration;
                duration = Mathf.Clamp01(duration);
                
                if (!tween.Handle(duration))
                {
	                world.DelEntity(tweenEntity);
	                continue;
                }

                if (duration >= 1)
                {
	                Debug.Log("Finished");
	                isComplete = true;
                }
            }
        }
    }
    
    public interface ITweenComponent
    {
        public bool Handle(float t);
    }

    public struct TweenSettings : IEcsAutoReset<TweenSettings>
    {
        public float Duration;
        public float CycleDuration;
        public float Delay;
        public bool IsPause;
        public bool IsComplete;

        public void AutoReset(ref TweenSettings c)
        {
            c.Duration = 0;
            c.Delay = 0;
            c.IsPause = false;
            c.IsComplete = false;
        }
    }
    
    /*public delegate float EasingHandler(float t);

    public static class TweenExtensions
    {
	    private static readonly EasingHandler[] Handlers =
        {
	        Linear,
            InQuad,
            OutQuad,
            InOutQuad,
            InCubic,
            OutCubic,
            InOutCubic,
            InQuart,
            OutQuart,
            InOutQuart,
            InQuint,
            OutQuint,
            InOutQuint,
            InSine,
            OutSine,
            InOutSine,
            InExpo,
            OutExpo,
            InOutExpo,
            InCirc,
            OutCirc,
            InOutCirc,
            InElastic,
            OutElastic,
            InOutElastic,
            InBack,
            OutBack,
            InOutBack,
            InBounce,
            OutBounce,
            InOutBounce
        };
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Evaluate(this EasingType easingType, float t)
        {
            return Handlers[(int) easingType](t);
        }

        public static float Linear(float t) => t;
        public static float InQuad(float t) => t * t;
		public static float OutQuad(float t) => 1 - InQuad(1 - t);
		public static float InOutQuad(float t)
		{
			if (t < 0.5) return InQuad(t * 2) / 2;
			return 1 - InQuad((1 - t) * 2) / 2;
		}
		
		public static float InCubic(float t) => t * t * t;
		public static float OutCubic(float t) => 1 - InCubic(1 - t);
		public static float InOutCubic(float t)
		{
			if (t < 0.5) return InCubic(t * 2) / 2;
			return 1 - InCubic((1 - t) * 2) / 2;
		}
		
		public static float InQuart(float t) => t * t * t * t;
		public static float OutQuart(float t) => 1 - InQuart(1 - t);
		public static float InOutQuart(float t)
		{
			if (t < 0.5) return InQuart(t * 2) / 2;
			return 1 - InQuart((1 - t) * 2) / 2;
		}
		
		public static float InQuint(float t) => t * t * t * t * t;
		public static float OutQuint(float t) => 1 - InQuint(1 - t);
		public static float InOutQuint(float t)
		{
			if (t < 0.5) return InQuint(t * 2) / 2;
			return 1 - InQuint((1 - t) * 2) / 2;
		}
		
		public static float InSine(float t) => (float)-Math.Cos(t * Math.PI / 2);
		public static float OutSine(float t) => (float)Math.Sin(t * Math.PI / 2);
		public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) / -2;
		public static float InExpo(float t) => (float)Math.Pow(2, 10 * (t - 1));
		public static float OutExpo(float t) => 1 - InExpo(1 - t);
		public static float InOutExpo(float t)
		{
			if (t < 0.5) return InExpo(t * 2) / 2;
			return 1 - InExpo((1 - t) * 2) / 2;
		}
		
		public static float InCirc(float t) => -((float)Math.Sqrt(1 - t * t) - 1);
		public static float OutCirc(float t) => 1 - InCirc(1 - t);
		public static float InOutCirc(float t)
		{
			if (t < 0.5) return InCirc(t * 2) / 2;
			return 1 - InCirc((1 - t) * 2) / 2;
		}
		
		public static float InElastic(float t) => 1 - OutElastic(1 - t);
		public static float OutElastic(float t)
		{
			float p = 0.3f;
			return (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - p / 4) * (2 * Math.PI) / p) + 1;
		}
		
		public static float InOutElastic(float t)
		{
			if (t < 0.5) return InElastic(t * 2) / 2;
			return 1 - InElastic((1 - t) * 2) / 2;
		}

		public static float InBack(float t)
		{
			float s = 1.70158f;
			return t * t * ((s + 1) * t - s);
		}
		
		public static float OutBack(float t) => 1 - InBack(1 - t);
		
		public static float InOutBack(float t)
		{
			if (t < 0.5) return InBack(t * 2) / 2;
			return 1 - InBack((1 - t) * 2) / 2;
		}

		public static float InBounce(float t) => 1 - OutBounce(1 - t);

		public static float OutBounce(float t)
		{
			float div = 2.75f;
			float mult = 7.5625f;

			if (t < 1 / div)
			{
				return mult * t * t;
			}
			else if (t < 2 / div)
			{
				t -= 1.5f / div;
				return mult * t * t + 0.75f;
			}
			else if (t < 2.5 / div)
			{
				t -= 2.25f / div;
				return mult * t * t + 0.9375f;
			}
			else
			{
				t -= 2.625f / div;
				return mult * t * t + 0.984375f;
			}
		}
		
		public static float InOutBounce(float t)
		{
			if (t < 0.5) return InBounce(t * 2) / 2;
			return 1 - InBounce((1 - t) * 2) / 2;
		}
    }*/
}