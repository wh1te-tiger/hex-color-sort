namespace Scripts
{
    public class WorldPositionConvertable : MonoConvertable<WorldPosition>
    {
        void Awake()
        {
            Value = new WorldPosition()
            {
                Value = transform.position
            };
        }
    }
}