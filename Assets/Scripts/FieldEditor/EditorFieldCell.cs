using UnityEngine;

namespace Scripts
{
    public class EditorFieldCell : MonoBehaviour
    {
        [SerializeField] private bool isLocked;
        [SerializeField] private int lockCondition;
        [SerializeField] private Hexes[] hexes;
        
        [SerializeField] private Coordinates _coordinates;

        public void SetCoordinates(int q, int r)
        {
            _coordinates = new Coordinates(q, r);
        }

        public void SetHexes(Hexes[] hexes)
        {
            this.hexes = hexes;
        }

        public void SetIsLocked(bool isLocked)
        {
            this.isLocked = isLocked;
        }

        public void SetLockCondition(int lockCondition)
        {
            this.lockCondition = lockCondition;
        }

        public CellData GetCellData()
        {
            return new CellData
            {
                coordinates = _coordinates,
                hexes = hexes,
                isLocked = isLocked,
                lockCondition = lockCondition
            };
        }

        private void OnDrawGizmos()
        {
            if(!isLocked) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, .25f);
        }
    }
}