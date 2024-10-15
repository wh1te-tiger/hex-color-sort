using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneService
    {
        private readonly AppSessionData _data;

        public SceneService(AppSessionData data)
        {
            _data = data;
        }

        public void LoadScene(AppStates appState)
        {
            _data.State = appState;
            SceneManager.LoadScene(appState.ToString());
        }
    }

    public enum AppStates
    {
        Lobby,
        Core
    }
}