using UnityEngine;
using UnityEngine.VFX;

namespace Scripts
{
    [CreateAssetMenu(fileName = "ViewSettings", menuName = "Configurations/Game Settings/View Settings", order = 0)]
    public class ViewSettings : ScriptableObject
    {
        [field: SerializeField] public GameObject CellPrefab { get; private set; }
        [field: SerializeField] public GameObject HexPrefab { get; private set; }
        [field: SerializeField] public Color BaseCellColor { get; private set; }
        [field: SerializeField] public Color MainMenuCellColor { get; private set; }
        [field: SerializeField] public Color HighlightedCellColor { get; private set; }
        [field: SerializeField] public VfxProvider CollapseEffect { get; private set; }
        
        [field: SerializeField] public float HexHorizontalSpeed { get; private set; }
        [field: SerializeField] public float HexVerticalSpeed { get; private set; }
        [field: SerializeField] public float HexSpacing { get; private set; }
        [field: SerializeField] public float HexFlightHeight { get; private set; }
        [field: SerializeField] public float ShiftDuration { get; private set; }
        [field: SerializeField] public float CollapseDuration { get; private set; }
        
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