using UnityEngine;

namespace Root
{
    public struct InputData
    {
        public Vector2 PointerPosition;
        public Vector2 Delta;
        public readonly Transform Target;

        public InputData(Transform target, Vector2 pointerPosition, Vector2 delta)
        {
            PointerPosition = pointerPosition;
            Delta = delta;
            Target = target;
        }
    }
}