using UniRx;

namespace Root
{
    public class GameObjectViewModel : EntityProvider
    {
        protected override void Setup()
        {
            ref var activeComponent = ref Add<Active>();
            var p = new BoolReactiveProperty();
            activeComponent.Property = p;
            
            p.Value = false;
            p
                .Subscribe(v => gameObject.SetActive(v))
                .AddTo(this);
        }
    }
}