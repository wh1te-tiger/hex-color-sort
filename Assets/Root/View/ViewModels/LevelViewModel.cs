using UnityEngine;

namespace Root
{
    public class LevelViewModel : MonoBehaviour
    {
        [field: SerializeField] public Transform FieldRoot { get; private set; }
        [field: SerializeField] public Transform SlotsRoot { get; private set; }
        [field: SerializeField] public Transform SlotsSpawnPos { get; private set; }
        [field: SerializeField] public Transform HexContainer { get; private set; }
    }
}