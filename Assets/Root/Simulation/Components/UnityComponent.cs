using UnityEngine;

namespace Root
{
    public struct UnityComponent<T> where T : Object
    {
        public T Component;
    }
}