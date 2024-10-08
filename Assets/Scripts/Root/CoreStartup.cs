using Creation;
using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class CoreStartup : MonoBehaviour
    {
        [SerializeField] private FieldSettings fieldSettings;
        [SerializeField] private ViewSettings viewSettings;
        [SerializeField] private ColorSettings colorSettings;
        [SerializeField] private Transform fieldRoot;
        [SerializeField] private Transform slots;
        
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
            
            var fieldService = new FieldService();
            var dragService = new DragService(viewSettings);
            var hexService = new HexService(_world);
            var gameFlowService = new GameFlowService(_world);
            var fieldFactory = new FieldFactory(_world, fieldService, fieldSettings);
            var hexFactory = new HexViewFactory(_world, viewSettings, null);
            
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
            _systems.Add(new CreateInitialHexesSystem(fieldService, fieldSettings));
            _systems.Add(new CreateHexesSystem(gameFlowService, colorSettings));
            _systems.Add(new CreateFieldViewSystem(viewSettings, fieldRoot));
            
            _systems.Add(new CheckDragOverCellSystem(gameFlowService, dragService, fieldService));
            _systems.Add(new MarkDraggingSystem());
            _systems.Add(new UnmarkDraggingSystem());
            _systems.Add(new TargetChangedEventSystem());
            _systems.Add(new DelaySystem());
            
            _systems.Add(new ReturnExecuteSystem(gameFlowService, viewSettings));
            _systems.Add(new RiseExecuteSystem(gameFlowService, viewSettings));
            _systems.Add(new DropExecuteSystem(gameFlowService, viewSettings));
            _systems.Add(new PickCellSystem(fieldService, hexService, gameFlowService));
            _systems.Add(new ShiftExecuteSystem(gameFlowService));
            _systems.Add(new CheckCollapseStateSystem(gameFlowService, hexService));
            _systems.Add(new CollapseExecuteSystem(gameFlowService));

            _systems.Add(new HighlightSystem(viewSettings));
            _systems.Add(new CreateHexViewSystem(hexFactory, colorSettings));
            _systems.Add(new ShiftViewSystem(gameFlowService, viewSettings));
            _systems.Add(new MoveViewSystem(gameFlowService));
            _systems.Add(new CollapseViewSystem(gameFlowService, viewSettings));
            _systems.Add(new HexOrderViewSystem(viewSettings));
            _systems.Add(new HighlightSystem(viewSettings));
            
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
