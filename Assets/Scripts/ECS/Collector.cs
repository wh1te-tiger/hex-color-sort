using System.Collections;
using System.Collections.Generic;

namespace Scripts
{
    internal class Collector : IEnumerable<int>
    {
        private readonly HashSet<int> _values = new();
        
        public bool Add(int entity)
        {
            return _values.Add(entity);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}