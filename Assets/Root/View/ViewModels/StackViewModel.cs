using UniRx;

namespace Root
{
    public class StackViewModel : EntityProvider, ISelectable
    {
        public bool IsSelected { get; set; }
        
        protected override void Setup()
        {
            this
                .ObserveEveryValueChanged(_ => IsSelected)
                .Subscribe(v =>
                {
                    if (v)
                    {
                        Add<SelectedTag>();
                    }
                    else
                    {
                        Del<SelectedTag>();
                    }
                })
                .AddTo(this);
        }
    }
}
