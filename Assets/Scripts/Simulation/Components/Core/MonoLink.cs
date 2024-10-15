using UnityEngine;

namespace Scripts
{
    public struct MonoLink<T> where T: Object
    {
        public T Value;
    }
}