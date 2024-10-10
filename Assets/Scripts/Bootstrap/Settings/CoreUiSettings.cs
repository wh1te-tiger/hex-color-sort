using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Core UI Settings", menuName = "Configurations/Game Settings/Core UI Settings", order = 0)]
    public class CoreUiSettings : ScriptableObject
    {
        [field: SerializeField] public GameObject Canvas { get; private set; }
        [field: SerializeField] public CoreWindows Windows { get; private set; }
    }
}