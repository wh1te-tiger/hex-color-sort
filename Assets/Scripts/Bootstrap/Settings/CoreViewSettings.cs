using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Core View Settings", menuName = "Configurations/Game Settings/View Settings/Core", order = 0)]
    public class CoreViewSettings : ViewSettings
    {
        [field: SerializeField] public Color HighlightedCellColor { get; private set; }
        [field: SerializeField] public Color LockedCellColor { get; private set; }
        [field: SerializeField] public VfxProvider CollapseEffect { get; private set; }
        
        [field: SerializeField] public float HexHorizontalSpeed { get; private set; }
        [field: SerializeField] public float HexVerticalSpeed { get; private set; }
        [field: SerializeField] public float HexFlightHeight { get; private set; }
        [field: SerializeField] public float CollapseDuration { get; private set; }
    }
}