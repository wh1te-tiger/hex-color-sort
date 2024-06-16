using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Root
{
    public class CellView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color baseColor;
        [SerializeField] private Color highlightColor;
        [SerializeField] private MeshRenderer meshRenderer;
        
        [field: SerializeField] public bool IsFree { get; set; }
        public bool SelectionEnabled { get; set; }
        
        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            
            this.ObserveEveryValueChanged(_ => SelectionEnabled)
                .Subscribe(v =>
                {
                    _propertyBlock.SetColor("_Color", v ? highlightColor : baseColor);
                    meshRenderer.SetPropertyBlock(_propertyBlock);
                })
                .AddTo(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SelectionEnabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SelectionEnabled = false;
        }
    }
}
