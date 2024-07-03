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
            ref var transformComponent = ref GetOrAdd<UnityComponent<Transform>>();
            transformComponent.Component = transform;
            
            var tr = positionRoot == null ? transform : positionRoot;
            
            ref var positionComponent = ref GetOrAdd<Position>();
            positionComponent.Property = new Vector3ReactiveProperty(transform.position);
            /*ref var rotationComponent = ref GetOrAdd<Rotation>();
            positionComponent.Value = new Vector3ReactiveProperty();*/
            
            positionComponent.Property
                .Subscribe(value =>
                {
                    tr.position = new Vector3(value.x, value.y, value.z);
                })
                .AddTo(this);
            
            ref var parentComponent = ref Add<Parent>();
            var property = new ReactiveProperty<Transform>();
            parentComponent.Property = property;
            
            property.Value = transform.parent;
            property
                .Subscribe(v => transform.parent = v)
                .AddTo(this);

            /*this.ObserveEveryValueChanged(_ => transform.parent)
                .Subscribe(v =>
                {
                    if (!ReferenceEquals(v, property.Value))
                    {
                        property.Value = v;
                    }
                })
                .AddTo(this);*/
        }
    }
}