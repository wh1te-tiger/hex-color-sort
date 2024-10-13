using UnityEngine;
using Zenject;

namespace Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private Levels levelSettings;
        
        public override void InstallBindings()
        {
            InstallSettings();
            InstallData();
            InstallMisc();
        }

        private void InstallSettings()
        {
            Container.BindInstance(levelSettings).AsSingle();
        }

        private void InstallData()
        {
            var appData = new AppSessionData(
                levelSettings.Value
            );
            Container.BindInstance(appData);
        }
        
        void InstallMisc()
        {
            Container.Bind<CoreDataFactory>().AsSingle();
            Container.Bind<SceneService>().AsSingle();
        }
    }
}