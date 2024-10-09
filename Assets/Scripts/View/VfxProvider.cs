using System;
using UniRx;
using UnityEngine;
using UnityEngine.VFX;

namespace Scripts
{
    [RequireComponent(typeof(VisualEffect))]
    public class VfxProvider : MonoBehaviour, IDisposable
    {
        private Action _onFinished;
        private IDisposable _disposable;
        private VisualEffect _vfx;
        private void Awake()
        {
            _vfx = GetComponent<VisualEffect>();

            _disposable = _vfx
                .ObserveEveryValueChanged(v => v.HasAnySystemAwake())
                .Where(v => !v)
                .Skip(1)
                .Subscribe(_ => { _onFinished?.Invoke(); });
        }

        public void Play()
        {
            _vfx.Play();
        }
        
        public void Stop()
        {
            _vfx.Stop();
        }

        public void Reinit()
        {
            _vfx.Reinit();
        }

        public void Subscribe(Action act)
        {
            _onFinished = act;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}