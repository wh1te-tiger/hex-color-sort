using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowPresenter : MonoBehaviour, IDisposable
    {
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;
                
                _enabled = value;
                gameObject.SetActive(value);
                if (value)
                {
                    _onEnableInternal?.Invoke(this);
                    OnShow();
                }
                else
                {
                    _onDisableInternal?.Invoke(this);
                    Disposables.Visibility.Clear();
                    OnHide();
                }
            }
        }
        public WindowTypes WindowType;
        public WindowPresenter ParentWindow;
        
        protected readonly PanelDisposables Disposables = new();
        private bool _isDisposed;
        private bool _enabled  = true;
        private Action<WindowPresenter> _onEnableInternal;
        private Action<WindowPresenter> _onDisableInternal;
        private CanvasGroup _canvasGroup;
        private const float FadeDuration = 0.5f;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        protected virtual void OnHide()
        {
            _canvasGroup.DOFade(0f, FadeDuration).Play();
        }

        protected virtual void OnShow()
        {
            _canvasGroup.DOFade(1f, FadeDuration).Play();
        }

        void OnDestroy()
        {
            TryDispose();
        }

        void TryDispose()
        {
            if (!Disposables.Visibility.IsDisposed) Disposables.Visibility.Clear();
            if (!Disposables.Lifecycle.IsDisposed) Disposables.Lifecycle.Clear();
            _isDisposed = true;
        }
        
        public void Dispose()
        {
            TryDispose();
        }

        public void SetSafeArea(SafeAreaOffset safeAreaOffset)
        {
            var rect = GetComponent<RectTransform>();
            
        }
    }
    
    public class PanelDisposables
    {
        public CompositeDisposable Lifecycle = new();
        public CompositeDisposable Visibility = new();
    }
}