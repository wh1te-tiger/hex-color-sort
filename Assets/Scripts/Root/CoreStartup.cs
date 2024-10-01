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

            var dragService = new DragService();
            var fieldService = new FieldService();
            var hexService = new HexService(_world);
            var gameFlowService = new GameFlowService(_world);
            var fieldFactory = new FieldFactory(_world, fieldService, fieldSettings);
            var hexFactory = new HexViewFactory(_world, viewSettings, null);
            
            _systems.Add(new InputSystem());
            _systems.Add(new HandleDragSystem(dragService));
            _systems.Add(new HandleDragStartedSystem(dragService));
            _systems.Add(new HandleDragEndedSystem(dragService));
            
            
            _systems.Add(new CreateFieldSystem(fieldFactory));
            _systems.Add(new CreateInitialHexesSystem(fieldService, fieldSettings));
            _systems.Add(new CreateHexesSystem(gameFlowService, colorSettings));
            _systems.Add(new CreateFieldViewSystem(viewSettings, fieldRoot));
            
            
            _systems.Add(new MarkDraggingSystem());
            _systems.Add(new UnmarkDraggingSystem());
            _systems.Add(new TargetChangedEventSystem());
            _systems.Add(new DelaySystem());
            _systems.Add(new ProcessSystem<ShiftProcess>());
            _systems.Add(new ProcessSystem<MoveProcess>());
            
            _systems.Add(new PickCellSystem(fieldService, hexService, gameFlowService));
            _systems.Add(new ShiftExecuteSystem(gameFlowService));
            _systems.Add(new ReturnHexesSystem(gameFlowService));
            _systems.Add(new CheckCollapseStateSystem(gameFlowService, hexService));
            
            _systems.Add(new CreateHexViewSystem(hexFactory, colorSettings));
            _systems.Add(new ShiftViewSystem(gameFlowService));
            _systems.Add(new MoveViewSystem(gameFlowService));
            _systems.Add(new HexOrderViewSystem());
            _systems.Add(new HighlightSystem(viewSettings));
            
            _systems.Init();
        }
        
        void Update()
        {
            _systems.Run();
        }
    }
}