using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class Colorable : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorPropertyId = Shader.PropertyToID("_BaseColor");

        private Color _currentColor;

        public Color Color
        {
            get => _currentColor;
            set
            {
                _currentColor = value;
                _propertyBlock.SetColor(ColorPropertyId, value);
                meshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public void TweenColor(Color target, float duration)
        {
            DOVirtual.Color(Color, target, duration, col =>
            {
                Color = col;
            });
        }

        void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            Color = Color.white;
        }
    }
}