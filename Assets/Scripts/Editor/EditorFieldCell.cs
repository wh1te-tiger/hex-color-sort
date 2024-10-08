using UnityEngine;

namespace Scripts
{
    public class EditorFieldCell : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Position { get; set; }
    }
}