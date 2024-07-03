using UnityEngine;

namespace Root
{
    public class LevelViewModel : MonoBehaviour
    {
        [SerializeField] private Transform fieldRoot;
        [field: SerializeField] public Transform SlotsRoot { get; private set; }
        [field: SerializeField] public Transform SlotsSpawnPos { get; private set; }
        
    }
}