using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Game Settings/Level Settings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField] public ColorSettings ColorSettings { get; private set; }
        [field: SerializeField] public GameObject ContainerPrefab { get; private set; }
        [field: SerializeField] public GameObject HexPrefab { get; private set; }
    }
}