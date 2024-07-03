using UnityEngine;

namespace Root
{
    public class ChildRootViewModel : EntityProvider
    {
        [SerializeField] private Transform root;
        protected override void Setup()
        {
            ref var parentComponent = ref Add<ChildRoot>();
            parentComponent = new ChildRoot(root);
        }
    }
}