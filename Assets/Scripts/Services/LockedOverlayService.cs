using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LockedOverlayService: IInitializable
    {
        #region Dependencies
        
        private readonly Camera _worldCamera = Camera.main;
        private readonly Camera _overlayCamera;
        private readonly Canvas _overlayCanvas;
        private readonly CoreUiSettings _uiSettings;

        #endregion
        
        private Transform _root;
        private readonly Dictionary<int, LockedOverlay> _overlays = new();
        
        public LockedOverlayService(CoreUiSettings uiSettings, Canvas overlayCanvas)
        {
            _uiSettings = uiSettings;
            _overlayCanvas = overlayCanvas;
            _overlayCamera = _overlayCanvas.worldCamera;
        }

        public void Initialize()
        {
            _overlayCanvas.worldCamera = _overlayCamera;
            _root = _overlayCanvas.transform;
        }

        public void AddLock(int id, Vector3 pos, int value)
        {
            var screen = _worldCamera.WorldToScreenPoint(pos);
            var localPos = _overlayCamera.ScreenToWorldPoint(screen);
            
            var overlay = Object.Instantiate(_uiSettings.LockedOverlay, _root).GetComponent<LockedOverlay>();
            
            overlay.Set(localPos, value);
            _overlays.Add(id, overlay);
        }

        public void RemoveLock(int id)
        {
            var overlay = _overlays[id];
            overlay.Remove();
        }
    }
}