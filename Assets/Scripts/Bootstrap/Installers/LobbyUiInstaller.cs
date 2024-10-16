using UnityEngine;

namespace Scripts
{
    public class LobbyUiInstaller : UiInstaller
    {
        private readonly LobbyUiSettings _settings;
        private readonly Signal _signal;

        public LobbyUiInstaller(LobbyUiSettings settings, Signal signal)
        {
            _settings = settings;
            _signal = signal;
        }

        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromComponentInNewPrefab(_settings.Canvas).AsSingle();
            Container.BindFactory<GameObject, object[], Transform, WindowPresenter, UiFactory>().FromFactory<UiFactory>();
            Container.Bind<UiService>().AsSingle();
            
            InstallWindows();
        }
        
        private void InstallWindows()
        {
            var windows = _settings.Windows;
            InstallWindow(windows.LobbyWindow, _signal);
        }
    }
}