using System.Collections.Generic;

namespace Root
{
    public class ContainerViewModel : EntityProvider, ISelectable
    {
        public void AddSelected()
        {
            Add<SelectedTag>();
        }
        
        public void AddInitRequest()
        {
            Add<InitRequest>();
        }
        
        protected override void Setup()
        {
            ref var hexesComponent = ref Add<Hexes>();
            hexesComponent.Value = new List<int>();
        }
    }
}
