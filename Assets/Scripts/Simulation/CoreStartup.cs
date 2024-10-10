using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CoreStartup : MonoBehaviour
    {
        [SerializeField] private LevelSettings levelSettings;
        [SerializeField] private CoreViewSettings coreViewSettings;
        [SerializeField] private ColorSettings colorSettings;
        [SerializeField] private Transform fieldRoot;
        [SerializeField] private Transform hexRoot;
        [SerializeField] private Transform vfxRoot;
        [SerializeField] private Transform slots;
        [SerializeField] private CoreWindow _coreWindow;
        
        private EcsWorld _world;
        private IEcsSystems _systems;
        
        private void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);

            foreach (var provider in slots.GetComponentsInChildren<EntityProvider>())
            {
                provider.Inject(_world);
                provider.TryGetEntity(out var e);
                _world.GetPool<Empty>().Add(e);
            }
            
            ///Services
            var fieldService = new FieldService();
            var dragService = new DragService(coreViewSettings);
            var hexService = new HexService(_world);
            var gameFlowService = new GameFlowService(_world);
            var levelService = new LevelService(_world, levelSettings);
            
            ///Factories
            var fieldFactory = new FieldFactory(_world, fieldService, levelSettings.Field);
            var hexFactory = new HexViewFactory(_world, coreViewSettings, hexRoot);
            var vfxFactory = new VfxFactory(coreViewSettings, vfxRoot);
            
            ///UI
            //_coreWindow.Inject(levelService);
            
#if UNITY_EDITOR
                // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
                // Регистрируем отладочные системы по контролю за текущей группой систем. 
                _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif

            _systems.Add(new InputSystem());
            _systems.Add(new HandleDragSystem(dragService));
            _systems.Add(new HandleDragStartedSystem(dragService));
            _systems.Add(new HandleDragEndedSystem());
            
            
            _systems.Add(new CreateFieldSystem(fieldFactory));
            _systems.Add(new CreateInitialHexesSystem(fieldService, levelSettings.Field));
            _systems.Add(new CreateHexesSystem(gameFlowService, colorSettings, hexFactory));
            _systems.Add(new CreateFieldViewSystem(coreViewSettings, fieldRoot));
            
            _systems.Add(new CheckDragOverCellSystem(gameFlowService, dragService, fieldService));
            _systems.Add(new MarkDraggingSystem());
            _systems.Add(new UnmarkDraggingSystem());
            _systems.Add(new TargetChangedEventSystem());
            _systems.Add(new DelaySystem());
            
            _systems.Add(new ReturnExecuteSystem(gameFlowService, coreViewSettings));
            _systems.Add(new RiseExecuteSystem(gameFlowService, coreViewSettings));
            _systems.Add(new DropExecuteSystem(gameFlowService, coreViewSettings));
            _systems.Add(new PickCellSystem(fieldService, hexService, gameFlowService));
            _systems.Add(new ShiftExecuteSystem(gameFlowService));
            _systems.Add(new CheckCollapseStateSystem(gameFlowService, hexService));
            _systems.Add(new CollapseExecuteSystem(gameFlowService));
            
            _systems.Add(new CreateHexViewSystem(colorSettings));
            _systems.Add(new ShiftViewSystem(gameFlowService, coreViewSettings));
            _systems.Add(new MoveViewSystem(gameFlowService));
            _systems.Add(new CollapseViewSystem(gameFlowService, coreViewSettings, hexFactory, vfxFactory));
            _systems.Add(new HexOrderViewSystem(coreViewSettings));
            _systems.Add(new HighlightSystem(coreViewSettings));
            
            _systems.Add(new ProcessSystem<ShiftProcess>());
            _systems.Add(new ProcessSystem<MoveProcess>());
            _systems.Add(new ProcessSystem<CollapseProcess>());
            
            _systems.Init();
        }
        
        void Update()
        {
            _systems.Run();
        }
    }
}
