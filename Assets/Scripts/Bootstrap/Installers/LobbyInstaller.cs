using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private LobbySettings settings;
        [SerializeField] private FieldSettings fieldSettings;
        [SerializeField] private LevelSceneData sceneData;
        
        private readonly InstallSettings _installInfo = new();
        private EcsWorld _world;
        private readonly Signal _startSessionRequest = new();

        public override void InstallBindings()
        {
            _installInfo.Container = () => Container;
            
            InstallSettings();
            InstallWorld();
            InstallFactories();
            InstallSystems();
            InstallServices();
            InstallUi();
            
            InstallMessagesTransporting();
        }
        
        private void InstallSettings()
        {
            Container.Bind<ViewSettings>().To<LobbyViewSettings>().FromInstance(settings.ViewSettings).AsSingle();
            Container.BindInstance(settings.ViewSettings.ColorSettings).AsSingle();
        }

        private void InstallWorld()
        {
            _world = new EcsWorld();
            Container.BindInstance(_world).AsSingle();
        }
        
        private void InstallFactories()
        {
            Container.Bind<FieldFactory>().AsSingle().WithArguments(fieldSettings);
            Container.Bind<HexFactory>().AsSingle().WithArguments(sceneData.HexRoot);
        }
        
        private void InstallServices()
        {
            Container.BindInterfacesTo<StartupHandler>().AsSingle().WithArguments(_startSessionRequest);
            Container.Bind<FieldService>().AsSingle();
            Container.Bind<HexService>().AsSingle();
            Container.Bind<ProcessService>().AsSingle();
        }
        
        private void InstallSystems()
        {
            //Creation
            Add<CreateFieldSystem>();
            Add<CreateLobbyHexes>(fieldSettings);
            Add<CreateFieldViewSystem>(sceneData.FieldRoot);
            Add<CreateLobbyUiSystem>();
            
            //Simulation
            Add<PickRandomCellSystem>();
            Add<HandleTopHexSystem>();
            Add<TargetChangedEventSystem>();
            Add<DelaySystem>();
            Add<ShiftExecuteSystem>();
            
            //View
            Add<CreateHexViewSystem>();
            Add<ShiftViewSystem>();
            Add<HexOrderViewSystem>();
            
            //Process
            Add<ProcessSystem<ShiftProcess>>();
            
#if UNITY_EDITOR
            Add<Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem>();
            Add<Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem>();
#endif
        }
        
        private void InstallUi()
        {
            Container.BindInstance(_startSessionRequest).AsSingle().WhenInjectedInto<LobbyUiInstaller>();
            Container.BindInstance(settings.UiSettings).AsSingle();
            Container.Bind(typeof(Canvas), typeof(UiService))
                .FromSubContainerResolve()
                .ByInstaller<LobbyUiInstaller>()
                .WithKernel()
                .AsSingle();
        }
        
        private void InstallMessagesTransporting()
        {
            Container.BindInterfacesTo<MessageTransporter>().AsSingle().WithArguments(_installInfo).NonLazy();
        }
        
        private void Add<TSystem>(params object[] optionalArguments) where TSystem : IEcsSystem
        {
            Container.Add<TSystem>(_installInfo, optionalArguments);
        }
    }
}