using UnityEngine;

namespace Root
{
    public struct ChildRoot
    {
        public readonly Transform Value;

        public ChildRoot(Transform value)
        {
            Value = value;
        }
    }
}