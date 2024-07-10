using System;
using System.Collections.Generic;
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
        [SerializeField] private float radius;
        [SerializeField] private float maxDistance;

        public bool IsFree => Get<Hexes>().Value.Count == 0;
        public bool IsHighlighted { get; set; }
        public new int EntityId => base.EntityId;
        
        private ObservableEventTrigger _trigger;
        
        void Awake()
        {
            _trigger = GetComponent<ObservableEventTrigger>();
            
            _trigger
                .OnPointerEnterAsObservable()
                .Subscribe(_ =>
                {
                    if(IsFree) IsHighlighted = true;
                })
                .AddTo(this);
            _trigger
                .OnPointerExitAsObservable()
                .Subscribe(_ => IsHighlighted = false)
                .AddTo(this);
        }

        protected override void Setup()
        {
            //TODO: Move to Highlight System 
            this.ObserveEveryValueChanged(_ => IsHighlighted)
                .Subscribe(v => colorable.SetColor(v ? highlightColor : baseColor))
                .AddTo(this);
            
            this
                .OnTriggerEnterAsObservable()
                .Subscribe(HandleContainerPlacement)
                .AddTo(this);

            ref var hexes = ref Add<Hexes>();
            hexes.Value = new List<int>();
        }

        public void AddNeighbors(int[] neighborsId)
        {
            ref var neighbors = ref Add<Neighbors>();
            neighbors.Value = neighborsId;
        }
        
        private void HandleContainerPlacement(Collider other)
        {
            //TODO: rewrite after hexes
            if (!other.TryGetComponent(out ContainerViewModel container)) return;
            
            container.AddUnloadRequest(EntityId);
        }
    }
}
