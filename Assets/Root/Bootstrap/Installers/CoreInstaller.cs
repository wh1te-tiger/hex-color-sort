using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Root
{
    public class CoreInstaller : MonoInstaller
    {
        #region InstallState

        private InstallSettings InstallInfo = new();
        private EcsWorld _world;
        #endregion
        
        
        public override void InstallBindings()
        {
            InstallInfo.Container = () => Container;
            InstallWorld();
            InstallSystems();
            InstallServices();
            InstallMics();
            InstallZenjectToEcsMessagesTransporting();
        }

        void InstallWorld()
        {
            _world = new EcsWorld();
            EcsUnityEvents.EcsWorld = _world;
            Container.BindInstance(_world).AsSingle();
        }

        void InstallSystems()
        {
            Add<OnDragEventsSystem>();
            Add<RemoveDragEventsSystem>();
            Add<RiseStackSystem>();
            //Add<DropStackSystem>();
            Add<HandleStackDropSystem>();
            Add<HandleDragSystem>();
            Add<MoveSystem>();
        }

        void InstallServices()
        {
            Container.Bind<DragService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelViewService>().AsSingle();
        }
        
        void InstallZenjectToEcsMessagesTransporting()
        {
            Container.BindInterfacesTo<ZenjectToEcsMessagesTransporter>().AsSingle().WithArguments(InstallInfo).NonLazy();
        }

        void InstallMics()
        {
            var cameraRoot = GameObject.Find("/CameraRoot");
            Container.Bind<Camera>().FromInstance(cameraRoot.GetComponentInChildren<Camera>()).AsSingle();
        }
        
        public void Add<TSystem>(params object[] optionalArguments) where TSystem : IEcsSystem
        {
            Container.Add<TSystem>(InstallInfo, optionalArguments);
        }
    }
}