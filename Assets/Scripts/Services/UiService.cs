using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Scripts
{
    public class UiService 
    {
        public bool IsInitialized { get; private set; }
        public readonly IObservable<WindowPresenter> WindowChanged;

        private readonly Canvas _canvas;
        private readonly List<WindowInstallInfo> _windowInstallInfos;

        private readonly UiFactory _uiFactory;
        private WindowPresenter[] _windows;
        private WindowPresenter _currentWindow;

        public UiService(UiFactory uiFactory, List<WindowInstallInfo> windowInstallInfos, Canvas canvas)
        {
            _uiFactory = uiFactory;
            _windowInstallInfos = windowInstallInfos;
            _canvas = canvas;
            WindowChanged = this.ObserveEveryValueChanged(_ => _currentWindow);
        }

        public void Initialize<TWindow>() where TWindow : WindowPresenter
        {
            var safeArea = CreateSafeArea();
            
            _windows = _windowInstallInfos
                .Select(installInfo => _uiFactory.Create(installInfo.presenter.gameObject, installInfo.parameters, safeArea))
                .ToArray();
            
            _currentWindow = GetWindow<TWindow>();
            //_currentWindow.SetSafeArea(safe);
            _currentWindow.Enabled = true;

            IsInitialized = true;
        }

        public TWindow GetWindow<TWindow>() where TWindow : WindowPresenter
        {
            return _windows.First(w => w is TWindow) as TWindow;
        }

        public void DisplayWindow<TWindow>(params object[] p) where TWindow : WindowPresenter
        {
            var window = GetWindow<TWindow>();
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
        
        public void CloseCurrentWindow()
        {
            _currentWindow.Enabled = false;
            var nextWindow = _windows.First(w => w.GetType() == _currentWindow.ParentWindow.GetType() );
            
            _currentWindow = nextWindow;
        }

        private Transform CreateSafeArea()
        {
            var safeAreaObj = new GameObject("[SafeArea]");
            safeAreaObj.transform.SetParent(_canvas.transform, false);
            var rect = safeAreaObj.AddComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var minAnchor = safeArea.position;
            var maxAnchor = minAnchor + safeArea.size;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rect.anchorMin = minAnchor;
            rect.anchorMax = maxAnchor;
            
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            
            return safeAreaObj.transform;
        }
        
        SafeAreaOffset GetSafeAreaOffset(Canvas canvas)
        {
            var safeAreaRect = Screen.safeArea;
            var scaleRatio = canvas.GetComponent<RectTransform>().rect.width / Screen.width;

            var left = safeAreaRect.xMin * scaleRatio;
            var right = (Screen.width - safeAreaRect.xMax) * scaleRatio;
            var top = (Screen.height - safeAreaRect.yMax) * scaleRatio;
            var bottom = safeAreaRect.yMin * scaleRatio;

            return new SafeAreaOffset(left, right, top, bottom);
        }
    }

    public struct SafeAreaOffset
    {
        public float Left { get; }
        public float Right { get; }
        public float Top { get; }
        public float Bottom { get; }

        public SafeAreaOffset(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}