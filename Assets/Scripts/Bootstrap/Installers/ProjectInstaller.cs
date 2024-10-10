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
        }

        private void InstallSettings()
        {
            Container.BindInstance(levelSettings).AsSingle();
        }
    }
}