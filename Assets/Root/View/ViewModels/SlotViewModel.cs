using UnityEngine;

namespace Root
{
    public class SlotViewModel : MonoBehaviour
    { 
        public bool IsEmpty => transform.childCount == 0;
    }
}