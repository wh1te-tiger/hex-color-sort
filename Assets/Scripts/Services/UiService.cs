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
        public IObservable<WindowPresenter> WindowChanged;

        private readonly Canvas _canvas;
        private readonly List<WindowInstallInfo> _windowInstallInfos;

        private readonly UiFactory _uiFactory;
        private WindowPresenter[] _windows;
        private WindowPresenter _currentWindow;

        public UiService(UiFactory uiFactory, List<WindowInstallInfo> windowInstallInfos)
        {
            _uiFactory = uiFactory;
            _windowInstallInfos = windowInstallInfos;
            WindowChanged = this.ObserveEveryValueChanged(_ => _currentWindow);
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
    }
}