using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LockedOverlayService: IInitializable
    {
        private Transform _root;
        
        public void Initialize()
        {
            var canvas = new GameObject("a", typeof(Canvas)).GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
        }
    }
}