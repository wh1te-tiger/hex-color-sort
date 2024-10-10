using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class UiService
    {
        public bool IsInitialized { get; private set; }

        private readonly Canvas _canvas;
        private readonly List<WindowInstallInfo> _windowInstallInfos;
        
        private readonly UiFactory _uiFactory;
        private WindowPresenter[] _windows;
        private WindowPresenter _currentWindow;

        public UiService(UiFactory uiFactory, List<WindowInstallInfo> windowInstallInfos)
        {
            _uiFactory = uiFactory;
            _windowInstallInfos = windowInstallInfos;
        }
        
        public void Initialize<TWindow>() where TWindow : WindowPresenter
        {
            _windows = _windowInstallInfos
                .Select(installInfo => _uiFactory.Create(installInfo.presenter.gameObject, installInfo.parameters))
                .ToArray();
            
            _currentWindow = GetWindow<TWindow>();
            _currentWindow.Enabled = true;

            IsInitialized = true;
        }

        public TWindow GetWindow<TWindow>() where TWindow : WindowPresenter
        {
            return _windows.First(w => w is TWindow) as TWindow;
        }
        
        public void DisplayWindow(WindowPresenter window, params object[] p)
        {
            DisplayWindowInternal(window, p);
        }

        private void DisplayWindowInternal(WindowPresenter w, object[] p = null)
        {
            if (!IsInitialized)
            {
                Debug.LogError($"Unable to display {w}-window, UIService is not initialized");
                return;
            }
            
            if (w.WindowType == WindowTypes.Fullscreen)
            {
                _currentWindow.Enabled = false;
            }
            else
            {
                w.transform.SetAsLastSibling();
            }
            
            _currentWindow = w;
            _currentWindow.Enabled = true;
        }
        
        /*SafeAreaOffset GetSafeAreaOffset(Canvas canvas)
        {
            var safeAreaRect = Screen.safeArea;
            var scaleRatio = canvas.GetComponent<RectTransform>().rect.width / Screen.width;

            var left = safeAreaRect.xMin * scaleRatio;
            var right = (Screen.width - safeAreaRect.xMax) * scaleRatio;
            var top = (Screen.height - safeAreaRect.yMax) * scaleRatio;
            var bottom = safeAreaRect.yMin * scaleRatio;

            return new SafeAreaOffset(left, right, top, bottom);
        }*/
    }
    
    public struct SafeAreaOffset
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public SafeAreaOffset(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}