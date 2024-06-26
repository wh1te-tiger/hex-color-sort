using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Root
{
    [RequireComponent(typeof(ObservableEventTrigger))]
    public class CellViewModel : Colorable
    {
        [SerializeField] private Color baseColor;
        [SerializeField] private Color highlightColor;
        
        [field: SerializeField] public bool IsFree { get; set; }
        
        public bool IsHighlighted { get; set; }
        private ObservableEventTrigger _trigger;
        
        protected override void Awake()
        {
            base.Awake();
            
            _trigger = GetComponent<ObservableEventTrigger>();
            
            _trigger
                .OnPointerEnterAsObservable()
                .Subscribe(_ => IsHighlighted = true)
                .AddTo(this);
            _trigger
                .OnPointerExitAsObservable()
                .Subscribe(_ => IsHighlighted = false)
                .AddTo(this);
            this.ObserveEveryValueChanged(_ => IsHighlighted)
                .Subscribe(v => SetColor(v ? highlightColor : baseColor))
                .AddTo(this);
        }
        
    }
}
