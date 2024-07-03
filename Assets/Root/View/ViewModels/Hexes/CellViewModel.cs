using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Root
{
    [RequireComponent(typeof(ObservableEventTrigger))]
    public class CellViewModel : EntityProvider
    {
        [SerializeField] private UnityEngine.Color baseColor;
        [SerializeField] private UnityEngine.Color highlightColor;
        [SerializeField] private Colorable colorable;
        
        [field: SerializeField] public bool IsFree { get; set; }
        
        public bool IsHighlighted { get; set; }
        private ObservableEventTrigger _trigger;
        
        void Awake()
        {
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
                .Subscribe(v => colorable.SetColor(v ? highlightColor : baseColor))
                .AddTo(this);

            this
                .OnTriggerEnterAsObservable()
                .Subscribe(v =>
                {
                    HandleStackPlacement();
                })
                .AddTo(this);
        }

        private void HandleStackPlacement()
        {
            Debug.Log($"{gameObject.GetInstanceID()}");
        }

        protected override void Setup()
        {
            
        }
    }
}
