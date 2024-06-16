using System;
using UnityEngine.EventSystems;

namespace Root
{
    public interface IDragHandler : IBeginDragHandler, UnityEngine.EventSystems.IDragHandler, IEndDragHandler
    {
        InputData InputData { get; }
        event Action<IDragHandler> DragStarted;
        event Action<IDragHandler> DragEnded;
    }
}