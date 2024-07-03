using System.Collections;
using System.Collections.Generic;

namespace Root
{
    internal class Collector : IEnumerable<int>
    {
        public readonly HashSet<int> Values = new();
        
        public int Count => Values.Count;

        public bool IsNotEmpty => Values.Count > 0;
        
        public bool Add(int entity)
        {
            return Values.Add(entity);
        }

        public void Remove(int entity)
        {
            Values.Remove(entity);
        }

        public void Clear()
        {
            Values.Clear();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}