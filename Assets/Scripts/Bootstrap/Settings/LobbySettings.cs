using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Lobby Settings", menuName = "Configurations/Game Settings/Lobby Settings", order = 0)]
    public class LobbySettings : ScriptableObject
    {
        [field: SerializeField] public LobbyViewSettings ViewSettings { get; private set; }
        [field: SerializeField] public LobbyUiSettings UiSettings { get; private set; }
    }
}