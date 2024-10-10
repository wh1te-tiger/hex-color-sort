using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Color Settings", menuName = "Configurations /Game Settings /Color Settings", order = 0)]
    public class ColorSettings : ScriptableObject
    {
        [SerializeField] private List<ColorPair> colors;
        [SerializeField] private ParticleSystem.MinMaxCurve countColorProbability = new(1, new AnimationCurve(), new AnimationCurve());

        private readonly Dictionary<ColorId, Color> _colors = new();

        public void OnEnable()
        {
            _colors.Clear();
            foreach (var colorPair in colors)
            {
                _colors.Add(colorPair.id, colorPair.color);
            }
        }

        public Color Get(ColorId colorId)
        {
            if (_colors.TryGetValue(colorId, out var color))
            {
                return color;
            }

            throw new ArgumentOutOfRangeException($"Color with ID:{color.ToString()} not found");

        }

        public IEnumerable<ColorId> GetRandomColors(int count)
        {
            var rnd = new Random();
            return Enumerable
                .Range(0, count)
                .Select(_ => rnd.Next(0, 1 + colors.Count - count))
                .OrderBy(x => x)
                .Select((x, i) => colors[x + i].id);
        }
        
        public int GetColorsCount(int count)
        {
            return Mathf.CeilToInt(countColorProbability.Evaluate(count));
        }
    }

    [Serializable]
    public struct ColorPair
    {
        public ColorId id;
        public Color color;
    }

    public enum ColorId
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Black,
        White,
        None
    }
}