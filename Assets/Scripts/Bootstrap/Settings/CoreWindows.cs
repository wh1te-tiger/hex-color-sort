using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Windows", menuName = "Configurations/Containers/Core Windows", order = 0)]
    public class CoreWindows : ScriptableObject
    {
        [field: SerializeField] public WindowPresenter CoreWindow { get; private set; }
        [field: SerializeField] public WindowPresenter WinWindow { get; private set; }
        [field: SerializeField] public WindowPresenter FailWindow { get; private set; }
    }
}