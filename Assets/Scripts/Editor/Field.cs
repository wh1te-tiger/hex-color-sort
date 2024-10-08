using UnityEngine;

namespace Scripts
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private FieldSettings fieldSettings;
        [SerializeField] private FieldType type;
        [SerializeField] private int dimension;
        [SerializeField] private EditorFieldCell cellPrefab;
        [SerializeField] private Transform root;
    }
}