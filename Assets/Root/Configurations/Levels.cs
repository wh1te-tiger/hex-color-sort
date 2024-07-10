using System.Collections.Generic;
using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Configurations /Levels", order = 0)]
    public class Levels : ScriptableObject
    {
        [field: SerializeField] public List<LevelSettings> Value { get; private set; }
    }
}