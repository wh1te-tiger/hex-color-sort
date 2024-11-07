using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts
{
    public class CoreWindow : WindowPresenter, IInitializable
    {
        #region ViewLinks

        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private Button settings;
        [SerializeField] private TMP_Text label;

        #endregion
        #region Dependencies

        [Inject] private LevelService _levelService;
        [Inject] private UiService _uiService;

        #endregion

        public void Initialize()
        {
            progressBar.SetBounds(_levelService.WinScore);
            _levelService
                .ObserveEveryValueChanged(s => s.Score)
                .Subscribe(v => progressBar.SetFillAmountUnclamped(v))
                .AddTo(Disposables.Lifecycle);
            _uiService.WindowChanged.Subscribe(WindowChanged).AddTo(Disposables.Lifecycle);
            settings.onClick.AddListener(()=>_uiService.DisplayWindow<SettingsWindow>());
            label.text = $"level {_levelService.LevelId}";
        }

        private void WindowChanged(WindowPresenter window)
        {
            settings.interactable = window is not SettingsWindow;
        }
    }
}