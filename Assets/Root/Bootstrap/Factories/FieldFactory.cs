using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Root
{
    public class FieldFactory
    {
        private readonly Levels _levels;
        private readonly CoreData _coreData;
        private readonly EcsWorld _ecsWorld;

        public FieldFactory(Levels levels, CoreData coreData, EcsWorld ecsWorld)
        {
            _levels = levels;
            _coreData = coreData;
            _ecsWorld = ecsWorld;
        }

        public GameObject Create()
        {
            var levelSettings = _levels.Value.First(l => l.Id == _coreData.LevelId);
            if (levelSettings == null)
            {
                levelSettings = _levels.Value[0];
                Debug.LogError($"Level with ID:{_coreData.LevelId} doesn't exist. Loading default level.");
            }

            var field = Object.Instantiate(levelSettings.FieldPrefab);
            var cells = field.GetComponentsInChildren<EntityConverter>();
            foreach (var cell in cells)
            {
                cell.Convert(_ecsWorld);
            }

            var colliders = new Collider[7];
            foreach (var cellViewModel in field.GetComponentsInChildren<CellViewModel>())
            {
                var size = Physics.OverlapSphereNonAlloc(cellViewModel.transform.position, 2, colliders, LayerMask.GetMask("Cell"));
                
                var res = colliders
                    .Take(size)
                    .Where(c => !c.gameObject.Equals(cellViewModel.gameObject))
                    .Select(c => c.GetComponent<CellViewModel>().EntityId)
                    .ToArray();
                cellViewModel.AddNeighbors(res);
            }
            
            return field;
        }
    }
}