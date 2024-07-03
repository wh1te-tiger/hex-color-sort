using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "Color Settings", menuName = "Game Settings / Color Settings", order = 0)]
    public class ColorSettings : ScriptableObject
    {
        [SerializeField] private List<ColorPair> colors;
        [SerializeField] private ParticleSystem.MinMaxCurve countColorProbability = new(1, new AnimationCurve(), new AnimationCurve());

        public UnityEngine.Color Get(ColorName colorName)
        {
            return colors.First(c => c.name == colorName).color;
        }

        public IEnumerable<ColorName> GetRandomColors(int count)
        {
            var rnd = new System.Random();
            return  Enumerable
                .Range(0, count)
                .Select(x => rnd.Next(0, 1 + colors.Count - count))
                .OrderBy(x => x)
                .Select((x, i) => colors[x + i].name);
        }

        public int GetColorsCount(int count)
        {
            return Mathf.CeilToInt(countColorProbability.Evaluate(count));
        }
    }

    [Serializable]
    public struct ColorPair
    {
        public ColorName name;
        public UnityEngine.Color color;
    }

    public enum ColorName
    {
        Red,
        Green,
        Blue,
        Yellow,
        Cian,
        Purple,
        Black,
        White
    }
}