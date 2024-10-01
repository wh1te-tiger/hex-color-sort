using Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test
{
    [RequireComponent(typeof(Colorable))]
    public class ColorableTest : MonoBehaviour
    {
        private Colorable _cololrable;

        private void Awake()
        {
            _cololrable = GetComponent<Colorable>();
        }

        private void Update()
        {
            _cololrable.Color = Random.ColorHSV();
        }
    }
}