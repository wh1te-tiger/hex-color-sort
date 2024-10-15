using System;
using Leopotam.EcsLite;
using UniRx;

namespace Scripts
{
    public class DisplayEndGameWindow : IEcsInitSystem, IDisposable
    {
        private readonly LevelService _levelService;
        private readonly UiService _uiService;
        
        private EcsFilter _emptyCellsFilter;
        private IDisposable _disposable;

        public DisplayEndGameWindow(LevelService levelService, UiService uiService)
        {
            _levelService = levelService;
            _uiService = uiService;
        }

        public void Init(IEcsSystems systems)
        {
            _disposable = _levelService.LevelState.Subscribe(OnStateChanged);
        }

        private void OnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Failed:
                    _uiService.DisplayWindow<FailWindow>();
                    break;
                case GameState.Win:
                    _uiService.DisplayWindow<WinWindow>();
                    break;
                case GameState.Playing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}