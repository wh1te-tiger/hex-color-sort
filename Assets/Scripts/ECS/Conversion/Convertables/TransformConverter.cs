using UnityEngine;

namespace Scripts
{
    public class TransformConverter : MonoConvertable<MonoLink<Transform>>
    {
        void Awake()
        {
            Value = new MonoLink<Transform>
            {
                Value = transform
            };
        }
    }
}