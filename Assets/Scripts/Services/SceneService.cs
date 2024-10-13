using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneService
    {
        public void LoadScene(AppStates appState, bool isFadeEnabled = true)
        {
            SceneManager.LoadScene(appState.ToString());
        }
    }

    public enum AppStates
    {
        Lobby,
        Core
    }
}