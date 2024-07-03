using UniRx;

namespace Root
{
    public struct Color
    {
        public ColorName Id;
        public ReactiveProperty<UnityEngine.Color> Property;
    }
}