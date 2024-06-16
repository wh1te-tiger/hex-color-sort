using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Root
{
    public class DragService : MonoBehaviour
    {
        [SerializeField] private List<MoveDragHandler> dragHandlers;
        
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
            
            foreach (var handler in dragHandlers)
            {
                this.ObserveEveryValueChanged(_ => handler.InputData)
                    .Skip(1)
                    .Subscribe(input => HandleDrag(handler, input))
                    .AddTo(this);
                handler.DragEnded += OnDragEnded;
            }
        }

        private void OnDragEnded(IDragHandler handler)
        {
            switch (handler)
            {
                case IMoveDragHandler:
                    var inputData = handler.InputData;
                    var screenPos = new Vector3(inputData.PointerPosition.x, inputData.PointerPosition.y, _cam.nearClipPlane);
                    var ray = _cam.ScreenPointToRay(screenPos);
                    if (Physics.Raycast(ray, out var hit, 20, LayerMask.GetMask("Cell")))
                    {
                        inputData.Target.SetParent(hit.transform);
                    }

                    inputData.Target.localPosition = new Vector3();
                    break;
            }
        }

        private void HandleDrag(IDragHandler handler, InputData inputData)
        {
            switch (handler)
            {
                case IMoveDragHandler:
                    var pos = DragPointToWorldPos(inputData.PointerPosition, -1f);
                    inputData.Target.position = new Vector3(pos.x, 1f, pos.y);
                    break;
            }
        }
        
        private Vector2 DragPointToWorldPos(Vector2 position, float distance)
        {
            var worldPoint = _cam.ScreenToWorldPoint(new Vector3(position.x, position.y, _cam.nearClipPlane));
            var plane = new Plane(Vector3.up, distance);
            var ray = _cam.ScreenPointToRay(position);
            if (plane.Raycast(ray, out var enter))
            {
                worldPoint = ray.GetPoint(enter);
            }
            return new Vector2(worldPoint.x, worldPoint.z);
        }
    }
}