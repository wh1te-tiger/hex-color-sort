using UnityEngine;

namespace Scripts
{
    public class LevelSceneData : MonoBehaviour
    {
        [field: SerializeField] public Transform FieldRoot {get; private set; }
        [field: SerializeField] public Transform HexRoot {get; private set; }
        [field: SerializeField] public Transform VFXRoot {get; private set; }
        [field: SerializeField] public Transform Slots {get; private set; }
        [field: SerializeField] public Canvas OverlayCanvas {get; private set; }
    }
}