using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private CoreSettings settings;
        [SerializeField] private LevelSettings levelSettings;
        [SerializeField] private LevelSceneData sceneData;
        [Inject] private AppSessionData _appData;
        
        private readonly InstallSettings _installInfo = new();
        private EcsWorld _world;
        
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
            Container.Bind<ViewSettings>().FromInstance(settings.CoreViewSettings).AsSingle();
            Container.BindInstance(settings.CoreViewSettings).AsSingle();
            Container.BindInstance(settings.CoreViewSettings.ColorSettings).AsSingle();

            Container.BindInstance(levelSettings).AsSingle();
            Container.BindInstance(levelSettings.Field).AsSingle();
        }
        
        private void InstallWorld()
        {
            _world = new EcsWorld();
            Container.BindInstance(_world).AsSingle();
        }
        
        private void InstallFactories()
        {
            Container.Bind<FieldFactory>().AsSingle();
            Container.Bind<HexFactory>().AsSingle().WithArguments(settings.CoreViewSettings, sceneData.HexRoot);
            Container.Bind<VfxFactory>().AsSingle().WithArguments(sceneData.VFXRoot);
        }

        private void InstallServices()
        {
            Container.Bind<FieldService>().AsSingle();
            Container.Bind<DragService>().AsSingle();
            Container.Bind<HexService>().AsSingle();
            Container.Bind<GameFlowService>().AsSingle();
            Container.Bind<LevelService>().AsSingle();
        }
        
        private void InstallSystems()
        {
            //Input
            Add<InputSystem>();
            Add<HandleDragStartedSystem>();
            Add<HandleDragSystem>();
            Add<HandleDragEndedSystem>();
            
            //Creation
            Add<CreateFieldSystem>();
            Add<CreateInitialHexesSystem>(_appData.SavedCoreSession);
            Add<CreateHexesSystem>();
            Add<CreateFieldViewSystem>(sceneData.FieldRoot);
            Add<CreateSlotsSystem>(sceneData.Slots);
            Add<CreateUiSystem>();
            
            //Simulation
            Add<CheckDragOverCellSystem>();
            Add<MarkDraggingSystem>();
            Add<UnmarkDraggingSystem>();
            Add<TargetChangedEventSystem>();
            Add<DelaySystem>();
            Add<ReturnExecuteSystem>();
            Add<RiseExecuteSystem>();
            Add<DropExecuteSystem>();
            Add<PickCellSystem>();
            Add<ShiftExecuteSystem>();
            Add<CheckCollapseStateSystem>();
            Add<CollapseExecuteSystem>();
            
            //View
            Add<HighlightSystem>();
            Add<CreateHexViewSystem>();
            Add<ShiftViewSystem>(settings.CoreViewSettings);
            Add<MoveViewSystem>();
            Add<CollapseViewSystem>();
            Add<HexOrderViewSystem>();
            
            //Process
            Add<ProcessSystem<ShiftProcess>>();
            Add<ProcessSystem<MoveProcess>>();
            Add<ProcessSystem<CollapseProcess>>();
            
#if UNITY_EDITOR
            Add<Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem>();
            Add<Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem>();
#endif
        }
        
        private void InstallUi()
        {
            Container.BindInstance(settings.CoreUiSettings).AsSingle();
            Container.Bind(typeof(Canvas), typeof(UiService))
                .FromSubContainerResolve()
                .ByInstaller<CoreUiInstaller>()
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