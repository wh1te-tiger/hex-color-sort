using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class MainStartup : MonoBehaviour
    {
        [SerializeField] private FieldSettings fieldSettings;
        [SerializeField] private ViewSettings viewSettings;
        [SerializeField] private ColorSettings colorSettings;
        
        private EcsWorld _world;
        private IEcsSystems _systems;
        private void Start()
        {
            _world = new EcsWorld();
            var hexPool = _world.GetPool<Hex>();
            var unorderedPool = _world.GetPool<Unordered>();
            _systems = new EcsSystems(_world);
            
            var fieldService = new FieldService();
            var hexService = new HexService(_world);
            var gameFlowService = new GameFlowService(_world);
            var hexFactory = new HexFactory(_world, viewSettings, colorSettings);
            
            _systems.Add(new ProcessSystem<ShiftProcess>());

            _systems.Add(new DelaySystem());
            _systems.Add(new HexOrderViewSystem(viewSettings));
            _systems.Add(new PickRandomCellSystem(fieldService, hexService, gameFlowService));
            _systems.Add(new ShiftExecuteSystem(gameFlowService));
            _systems.Add(new TargetChangedEventSystem());
            _systems.Add(new ShiftViewSystem(gameFlowService, viewSettings));
            _systems.Init();
            
            foreach (var cellData in fieldSettings.cells)
            {
                var cellEntity = fieldService.GetCellEntity(cellData.coordinates);
                
                if (cellData.hexes.Length == 0)
                {
                    _world.GetPool<Empty>().Add(cellEntity);
                    continue;
                }

                var index = 0;
                foreach (var hexData in cellData.hexes)
                {
                    for (var i = 0; i < hexData.count; i++)
                    {
                        var e = hexFactory.Create(_world.PackEntity(cellEntity), hexData.colorId);
                        
                        ref var hex = ref hexPool.Get(e);
                        hex.Index = index;
                        unorderedPool.Add(e);
                        index++;
                    }
                }
                
                ref var cell = ref _world.GetPool<Cell>().Get(cellEntity);
                cell.Count = index;
            }
        }

        private void Update()
        {
            _systems.Run();
        }
    }
}