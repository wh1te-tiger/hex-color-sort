using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace Scripts
{
    public class LobbyWindow : WindowPresenter, IInitializable
    {
        [SerializeField] private Button playButton;
        [SerializeField] private TMP_Text levelText;
        
        [Inject] private AppSessionData _appData;
        [Inject] private Signal _startSessionRequest;


        public void Initialize()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            levelText.text = _appData.NextLevelID.ToString();
        }

        private void OnPlayButtonClicked()
        {
            _startSessionRequest?.Invoke();
        }
    }
}