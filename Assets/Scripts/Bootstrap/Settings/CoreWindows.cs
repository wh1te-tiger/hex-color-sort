using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Windows", menuName = "Configurations/Containers/Core Windows", order = 0)]
    public class CoreWindows : ScriptableObject
    {
        [field: SerializeField] public WindowPresenter Core { get; private set; }
        [field: SerializeField] public WindowPresenter Win { get; private set; }
        [field: SerializeField] public WindowPresenter Fail { get; private set; }
        [field: SerializeField] public WindowPresenter Settings { get; private set; }
    }
}