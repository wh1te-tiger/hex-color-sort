using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CoreWindow : WindowPresenter, IInitializable
    {
        #region ViewLinks

        [SerializeField] private ProgressBar _progressBar;

        #endregion

        #region Dependencies

        [Inject] private LevelService _levelService;

        #endregion

        public void Initialize()
        {
            _progressBar.SetBounds(_levelService.WinScore);
            _levelService
                .ObserveEveryValueChanged(s => s.Score)
                .Subscribe(v => _progressBar.SetFillAmountUnclamped(v))
                .AddTo(Disposables.Lifecycle);
        }
    }
}