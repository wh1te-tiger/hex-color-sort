namespace Scripts
{
    public class SlotConvertable : MonoConvertable<Slot>
    {
        void Awake()
        {
            Value = new Slot();
        }
    }
}