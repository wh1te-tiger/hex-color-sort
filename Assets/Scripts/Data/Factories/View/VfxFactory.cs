using UnityEngine;
using UnityEngine.VFX;

namespace Scripts
{
    public class VfxFactory : FactoryWithPool<VfxProvider>
    {
        private readonly ViewSettings _viewSettings;
        private readonly Transform _root;

        public VfxFactory(ViewSettings viewSettings, Transform root)
        {
            _viewSettings = viewSettings;
            _root = root;
        }

        protected override VfxProvider CreateFunction()
        {
            var vfx = Object.Instantiate(_viewSettings.CollapseEffect, _root);
            return vfx;
        }
        
        protected override void OnReleaseFunction(VfxProvider element)
        {
            base.OnReleaseFunction(element);
            element.gameObject.SetActive(false);
            element.Stop();
        }

        protected override void OnGetFunction(VfxProvider element)
        {
            base.OnGetFunction(element);
            element.gameObject.SetActive(true);
            element.Reinit();
        }
    }
}