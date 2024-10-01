using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "ViewSettings", menuName = "Game Settings/View Settings", order = 0)]
    public class ViewSettings : ScriptableObject
    {
        [field: SerializeField] public GameObject CellPrefab { get; private set; }
        [field: SerializeField] public GameObject HexPrefab { get; private set; }
        [field: SerializeField] public Color BaseCellColor { get; private set; }
        [field: SerializeField] public Color HighlightedCellColor { get; private set; }
        public float HexSize { get; private set; }
        
        public void OnEnable()
        {
            var r = CellPrefab.GetComponentInChildren<Renderer>();
            HexSize = r.bounds.extents.x;
        }
    }
}