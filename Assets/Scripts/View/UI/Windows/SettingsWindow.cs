using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts
{
    public class SettingsWindow : WindowPresenter, IInitializable
    {
        #region ViewLinks

        [SerializeField] private Button backButton;
        [SerializeField] private Button quitButton;

        #endregion
        #region Dependencies

        [Inject] private Signal _returnToLobbyRequest;
        [Inject] private UiService _uiService;

        #endregion
        
        public void Initialize()
        {
            backButton.onClick.AddListener(() => _uiService.CloseCurrentWindow());
            quitButton.onClick.AddListener(() => _returnToLobbyRequest?.Invoke());
        }
    }
}