using System;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Level.", menuName = "Configurations/Game Settings/LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public FieldSettings Field { get; private set; }
        [field: SerializeField] public WinCondition WinCondition { get; private set; }
    }

    [Serializable]
    public struct WinCondition
    {
        public int Score;
    }
}