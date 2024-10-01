using System;

namespace Scripts
{
    [Serializable]
    public struct Coordinates
    {
        public int q;
        public int r;
        [NonSerialized] public int s;

        public Coordinates(int q, int r)
        {
            this.q = q;
            this.r = r;
            s = -this.q - this.r;
        }

        public Coordinates(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public static Coordinates operator +(Coordinates a, Coordinates b) => new(a.q + b.q, a.r + b.r);

        public override bool Equals(object obj)
        {
            return obj is Coordinates c && Equals(c);
        }

        public bool Equals(Coordinates other)
        {
            return q == other.q && r == other.r;
        }

        public override string ToString()
        {
            return $"({q},{r},{s})";
        }

        public override int GetHashCode()
        {
            int num1 = q;
            int hashCode = num1.GetHashCode();
            num1 = r;
            int num2 = num1.GetHashCode() << 2;
            return hashCode ^ num2;
        }
    }

    public struct Fractional
    {
        public float q;
        public float r;
        public float s;

        public Fractional(float q, float r)
        {
            this.q = q;
            this.r = r;
            s = -q - r;
        }

        public Fractional(float q, float r, float s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }
    }
}