using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Root
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private LevelSettings levelSettings;
        
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
            Container.BindInstance(levelSettings).AsSingle();
            Container.BindInstance(levelSettings.ColorSettings).AsSingle();
        }

        void InstallFactories()
        {
            Container.Bind<ContainerFactory>().AsSingle();
            Container.Bind<HexFactory>().AsSingle();
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
            Add<SpawnContainersSystem>();
            Add<PlaceContainersSystem>();
            Add<FillContainerSystem>();
            Add<OrganizeHexPositionSystem>();
            Add<ActivateContainerSystem>();
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