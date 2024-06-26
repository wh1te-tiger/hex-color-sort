using UnityEngine;

namespace Root
{
    public struct Destination
    {
        private Vector3 _value;

        public Vector3 Value
        {
            readonly get => _value;
            set
            {
                _value = value;
                IsCompleted = false;
            }
        }

        public bool IsCompleted { get; set; }
    }
}