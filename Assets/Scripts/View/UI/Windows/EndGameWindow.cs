﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts
{
    public abstract class EndGameWindow : WindowPresenter, IInitializable 
    {
        #region ViewLinks

        [SerializeField] private Button button;
        [SerializeField] private TMP_Text levelText;

        #endregion
        #region Dependencies

        [Inject] protected SoundService SoundService;
        [Inject] protected UiAudioSource UiAudioSource;
        [Inject] private Signal _returnToLobbyRequest;
        [Inject] private AppSessionData _data;

        #endregion
        
        public void Initialize()
        {
            levelText.text = (_data.NextLevelID + 1).ToString();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _returnToLobbyRequest?.Invoke();
        }
    }
}