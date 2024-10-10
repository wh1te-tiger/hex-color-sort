using UnityEngine;

namespace Scripts
{
    public abstract class ViewSettings : ScriptableObject
    {
        [field: SerializeField] public ColorSettings ColorSettings { get; private set; }
        [field: SerializeField] public GameObject CellPrefab { get; private set; }
        [field: SerializeField] public GameObject HexPrefab { get; private set; }
        [field: SerializeField] public Color BaseCellColor { get; private set; }
        [field: SerializeField] public float HexSpacing { get; private set; }
        [field: SerializeField] public float ShiftDuration { get; private set; }
        
        public float CellWidth { get; private set; }
        public float CellHeight { get; private set; }
        public float HexHeight { get; private set; }
        
        public void OnEnable()
        {
            var r = CellPrefab.GetComponentInChildren<Renderer>();
            CellWidth = r.bounds.extents.x;
            CellHeight = r.bounds.extents.y;
            
            r = HexPrefab.GetComponentInChildren<Renderer>();
            HexHeight = r.bounds.extents.y;
        }
    }
}