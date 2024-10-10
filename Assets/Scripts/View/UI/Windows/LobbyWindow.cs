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


        public void Initialize()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            Debug.Log("clicked");
        }
    }
}