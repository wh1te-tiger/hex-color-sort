using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(Colorable))]
    public class ColorableConverter : MonoConvertable<MonoLink<Colorable>>
    {
        void Awake()
        {
            Value = new MonoLink<Colorable>
            {
                Value = GetComponent<Colorable>()
            };
        }
    }
}