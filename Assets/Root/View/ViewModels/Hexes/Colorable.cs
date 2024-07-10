using UniRx;
using UnityEngine;

namespace Root
{
    public class Colorable : EntityProvider
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
        
        protected override void Setup()
        {
            ref var colorComponent = ref GetOrAdd<Color>();
            colorComponent.Property = new ReactiveProperty<UnityEngine.Color>();
            colorComponent.Property
                .ObserveEveryValueChanged(p=> p.Value)
                .Skip(1)
                .Subscribe(SetColor)
                .AddTo(this);
        }

        public void SetColor(UnityEngine.Color color)
        {
            _propertyBlock.SetColor(ColorPropertyId, color);
            meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}