using UnityEngine;
using UnityEngine.VFX;

namespace Scripts
{
    public class VfxFactory : FactoryWithPool<VfxProvider>
    {
        private readonly CoreViewSettings _coreViewSettings;
        private readonly Transform _root;

        public VfxFactory(CoreViewSettings coreViewSettings, Transform root)
        {
            _coreViewSettings = coreViewSettings;
            _root = root;
        }

        protected override VfxProvider CreateFunction()
        {
            var vfx = Object.Instantiate(_coreViewSettings.CollapseEffect, _root);
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