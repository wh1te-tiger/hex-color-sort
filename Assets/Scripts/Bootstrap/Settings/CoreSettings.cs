using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Core Settings", menuName = "Configurations/Game Settings/Core Settings", order = 0)]
    public class CoreSettings : ScriptableObject
    {
        [field: SerializeField] public CoreViewSettings CoreViewSettings { get; private set; }
        [field: SerializeField] public CoreUiSettings CoreUiSettings { get; private set; }
    }
}