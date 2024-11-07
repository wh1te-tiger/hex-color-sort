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
        [SerializeField] private LayoutGroup textGroup;
        
        [Inject] private AppSessionData _appData;
        [Inject] private Signal _startSessionRequest;


        public void Initialize()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            levelText.text = _appData.NextLevelID.ToString();
            textGroup.CalculateLayoutInputHorizontal();
            Debug.Log("hellow");
        }

        private void OnPlayButtonClicked()
        {
            _startSessionRequest?.Invoke();
        }
    }
}