using UnityEngine;

namespace Scripts
{
    public class Colorable : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorPropertyId = Shader.PropertyToID("_BaseColor");

        public Color Color
        {
            set
            {
                _propertyBlock.SetColor(ColorPropertyId, value);
                meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            Color = Color.white;
        }
    }
}