using UnityEngine;
using Zenject;

namespace Root
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private CoreSettings coreSettings;
        public override void InstallBindings()
        {
            InstallSettings();
            InstallData();
        }

        private void InstallSettings()
        {
            Container.BindInstance(coreSettings).AsSingle();
        }

        private void InstallData()
        {
            var coreData = new CoreData
            {
                LevelId = coreSettings.Levels.Value[0].Id
            };
            Container.BindInstance(coreData);
        }
    }
}