using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Field : MonoBehaviour
    {
        [field: SerializeField] public FieldSettings FieldSettings { get; set; }
        [SerializeField] private int dimension;
        [SerializeField] private FieldType type;
        [SerializeField] private EditorFieldCell cellPrefab;
        [SerializeField] private Transform root;
        
        private readonly Coordinates[] _directions =
        {
            new(1, 0),
            new(-1, 0),
            new(0,1),
            new(0,-1),
            new(1,-1),
            new(-1,1)
        };

        public bool HasSettings => FieldSettings != null;

        #region API

        public void CreateField()
        {
            root.DestroyChildrenImmediate();
            switch (type)
            {
                case FieldType.Hexagon:
                    CreateHexagonalField();
                    break;
                case FieldType.Rectangle:
                    CreateRectangleField();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SaveField()
        {
            FieldSettings.cells = root.GetComponentsInChildren<EditorFieldCell>().Select(c => c.GetCellData()).ToArray();
        }
        
        public void LoadField()
        {
            root.DestroyChildrenImmediate();
            CreateFieldInstance(FieldSettings.cells);
        }

        #endregion
        
        private void CreateHexagonalField()
        {
            var coordinates = new HashSet<Coordinates> { new(0, 0) };

            for (var i = 0; i < dimension; i++)
            {
                var rawRes = coordinates.SelectMany(GetNeighbors).ToArray();
                
                foreach (var c in rawRes)
                {
                    coordinates.Add(c);
                }
            }

            CreateFieldInstance(coordinates);
        }

        private void CreateRectangleField()
        {
            var coordinates = new List<Coordinates>();
            for (var i = -dimension; i <= dimension; i++)
            {
                for (var j = -dimension; j <= dimension; j++)
                {
                    var q = j;
                    var r = i - (q - (q & 1)) / 2;
                    coordinates.Add(new Coordinates(q,r));
                }
            }
            
            CreateFieldInstance(coordinates);
        }

        private void CreateFieldInstance(IEnumerable<Coordinates> coordinates)
        {
            var r = cellPrefab.GetComponentInChildren<Renderer>();
            var width = r.bounds.extents.x;
            
            foreach (var cor in coordinates)
            {
                var cell = CreateCell(cor, width);
                cell.SetCoordinates(cor.q, cor.r);
            }
        }

        private void CreateFieldInstance(CellData[] cells)
        {
            var r = cellPrefab.GetComponentInChildren<Renderer>();
            var width = r.bounds.extents.x;
            
            foreach (var cellData in cells)
            {
                var cell = CreateCell(cellData.coordinates, width);
                cell.SetCoordinates(cellData.coordinates.q, cellData.coordinates.r);
                cell.SetHexes(cellData.hexes);
                cell.SetIsLocked(cellData.isLocked);
                cell.SetLockCondition(cellData.lockCondition);
            }
        }

        private EditorFieldCell CreateCell(Coordinates cor, float width)
        {
            var x = width * 3 / 2f * cor.q;
            var z = Mathf.Sqrt(3) / 2 * cor.q + Mathf.Sqrt(3) * cor.r;
            
            return Instantiate(cellPrefab, new Vector3(x,0,z), Quaternion.identity, root);
        }

        private IEnumerable<Coordinates> GetNeighbors(Coordinates cor)
        {
            return _directions.Select(d => cor + d);
        }
    }
    
    enum FieldType
    {
        Hexagon,
        Rectangle
    }
}