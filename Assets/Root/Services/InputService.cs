/*using UnityEngine;

namespace Root.Services
{
    public class InputService : MonoBehaviour
    {
        public InputData InputData { get; private set; } = new();
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var pointerPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                var delta = pointerPosition - InputData.PointerPosition;
                InputData = new InputData(pointerPosition, delta);
                //Debug.Log($"{InputData.PointerPosition}, {InputData.Delta}");
            }

            if (Input.GetMouseButtonUp(0))
            {
                InputData = default;
            }
        }
    }

    
}*/