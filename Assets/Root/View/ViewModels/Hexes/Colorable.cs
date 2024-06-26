using UnityEngine;

namespace Root
{
    public class Colorable : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        protected virtual void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetColor(Color color)
        {
            _propertyBlock.SetColor(ColorPropertyId, color);
            meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}