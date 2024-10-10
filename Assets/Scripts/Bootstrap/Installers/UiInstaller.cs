using Zenject;

namespace Scripts
{
    public abstract class UiInstaller : Installer<UiInstaller>
    {
        protected void InstallWindow<TWindow>(TWindow presenter, params object[] parameters) where TWindow : WindowPresenter
        {
            Container.BindInstance(new WindowInstallInfo(presenter, parameters)).AsCached();
        }
    }
    
    public class WindowInstallInfo
    {
        public WindowPresenter presenter;
        public object[] parameters;

        public WindowInstallInfo(WindowPresenter presenter, object[] parameters)
        {
            this.presenter = presenter;
            this.parameters = parameters;
        }
    }
}