using System.Linq;
using UnityEngine;

namespace Scripts
{
    public static class GameObjectExtensions
    {
        public static void DestroyChildren(this Transform t) {
            t.Cast<Transform>().ToList().ForEach(c => Object.Destroy(c.gameObject));
        }
 
        public static void DestroyChildrenImmediate(this Transform t) {
            t.Cast<Transform>().ToList().ForEach(c => Object.DestroyImmediate(c.gameObject));
        }
    }
}