using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Lobby UI Settings", menuName = "Configurations/Game Settings/Lobby UI Settings", order = 0)]
    public class LobbyUiSettings : ScriptableObject
    {
        [field: SerializeField] public GameObject Canvas { get; private set; }
        [field: SerializeField] public LobbyWindows Windows { get; private set; }
    }
}