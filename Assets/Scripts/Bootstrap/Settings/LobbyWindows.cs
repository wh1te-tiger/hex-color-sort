using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Lobby Windows", menuName = "Configurations/Containers/Lobby Windows", order = 0)]
    public class LobbyWindows : ScriptableObject
    {
        [field: SerializeField] public WindowPresenter LobbyWindow { get; private set; }
    }
}