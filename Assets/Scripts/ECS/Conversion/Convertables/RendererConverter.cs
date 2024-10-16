using UnityEngine;

namespace Scripts
{
    public class RendererConverter : MonoConvertable<MonoLink<Renderer>>
    {
        void Awake()
        {
            Value = new MonoLink<Renderer>
            {
                Value = GetComponentInChildren<Renderer>()
            };
        }
    }
}