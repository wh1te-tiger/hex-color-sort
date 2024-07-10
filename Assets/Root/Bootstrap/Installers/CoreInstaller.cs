using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Root
{
    public class CoreInstaller : MonoInstaller
    {
        [Inject] private CoreSettings coreSettings;
        
        #region InstallState

        private InstallSettings InstallInfo = new();
        private EcsWorld _world;
        
        #endregion
        
        
        public override void InstallBindings()
        {
            InstallInfo.Container = () => Container;
            InstallSettings();
            InstallFactories();
            InstallWorld();
            InstallSystems();
            InstallServices();
            InstallMics();
            InstallZenjectToEcsMessagesTransporting();
        }
        
        void InstallSettings()
        {
            Container.BindInstance(coreSettings.ColorSettings).AsSingle();
            Container.BindInstance(coreSettings.Levels).AsSingle();
        }

        void InstallFactories()
        {
            Container.BindInterfacesAndSelfTo<ContainerFactory>().AsSingle();
            Container.Bind<HexFactory>().AsSingle();
            Container.Bind<FieldFactory>().AsSingle();
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
            Add<ContainerRiseSystem>();
            Add<ContainerDropSystem>();
            Add<HandleDragSystem>();
            Add<ContainerSpawnSystem>();
            Add<ContainerFillSystem>();
            Add<ContainerHexSetPosition>();
            Add<ContainerPlaceSystem>();
            Add<ContainerActivateSystem>();
            Add<ContainerUnloadSystem>();
            Add<ContainerDeactivateSystem>();
            Add<FindReceiverSystem>();
            Add<AddSourceDelaySystem>();
            Add<HexShiftSystem>();
            Add<OrganizeHexPositionSystem>();
            Add<DelaySystem>();
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