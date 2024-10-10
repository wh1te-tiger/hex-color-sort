using UnityEngine;

namespace Scripts
{
    public class CoreUiInstaller : UiInstaller
    {
        private readonly CoreUiSettings _settings;

        public CoreUiInstaller(CoreUiSettings settings)
        {
            _settings = settings;
        }

        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromComponentInNewPrefab(_settings.Canvas).AsSingle();
            Container.BindFactory<GameObject, object[], WindowPresenter, UiFactory>().FromFactory<UiFactory>();
            Container.Bind<UiService>().AsSingle();
            
            InstallWindows();
        }

        private void InstallWindows()
        {
            var windows = _settings.Windows;
            InstallWindow(windows.CoreWindow);
            //InstallWindow(windows.FailWindow);
            //InstallWindow(windows.WinWindow);
        }
    }
}