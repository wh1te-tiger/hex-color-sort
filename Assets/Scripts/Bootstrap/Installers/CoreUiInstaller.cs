using UnityEngine;

namespace Scripts
{
    public class CoreUiInstaller : UiInstaller
    {
        private readonly CoreUiSettings _settings;
        private readonly Signal _signal;

        public CoreUiInstaller(CoreUiSettings settings, Signal signal)
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
            InstallWindow(windows.Core);
            InstallWindow(windows.Win, _signal);
            InstallWindow(windows.Fail, _signal);
            InstallWindow(windows.Settings, _signal);
        }
    }
}