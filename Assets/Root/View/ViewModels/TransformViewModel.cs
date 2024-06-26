using UniRx;
using UnityEngine;

namespace Root
{
    public class TransformViewModel : EntityProvider
    {
        [SerializeField] Transform positionRoot;
        [SerializeField] Transform rotationRoot;
        
        protected override void Setup()
        {
            var tr = positionRoot == null ? transform : positionRoot;
            
            ref var positionComponent = ref GetOrAdd<Position>();
            positionComponent.Value = new Vector3ReactiveProperty(transform.position);
            /*ref var rotationComponent = ref GetOrAdd<Rotation>();
            positionComponent.Value = new Vector3ReactiveProperty();*/
            
            positionComponent.Value
                .ObserveEveryValueChanged(v=>v.Value)
                .Subscribe(value =>
                {
                    tr.position = new Vector3(value.x, value.y, value.z);
                })
                .AddTo(this);
            
            ref var parentComponent = ref Add<Parent>();
            parentComponent.Value = transform.parent;
            this
                .ObserveEveryValueChanged(_ => transform.parent)
                .Subscribe(v =>
                {
                    ref var p = ref Get<Parent>();
                    p.Value = transform.parent;
                });
        }
    }
}