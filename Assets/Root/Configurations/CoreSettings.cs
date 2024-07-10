using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configurations /Game Settings /Core Settings", order = 0)]
    public class CoreSettings : ScriptableObject
    {
        [field: SerializeField] public ColorSettings ColorSettings { get; private set; }
        [field: SerializeField] public Levels Levels { get; private set; }
        [field: SerializeField] public GameObject ContainerPrefab { get; private set; }
        [field: SerializeField] public GameObject HexPrefab { get; private set; }
    }
}