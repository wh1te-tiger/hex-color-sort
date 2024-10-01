using UnityEngine;

namespace Scripts
{
    public class HeightOffsetConvertable : MonoConvertable<HeightOffset>
    {
        [SerializeField] private float value;
        
        void Awake()
        {
            Value = new HeightOffset
            {
                Value = value
            };
        }
    }
}