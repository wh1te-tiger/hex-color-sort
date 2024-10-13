using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Levels", menuName = "Configurations/Containers/Levels", order = 0)]
    public class Levels : ScriptableObject
    {
        [field: SerializeField] public LevelSettings[] Value { get; private set; }
    }
}