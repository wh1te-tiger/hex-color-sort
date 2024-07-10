using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Configurations /Game Settings /Level Settings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public GameObject FieldPrefab { get; private set; }
    }
}