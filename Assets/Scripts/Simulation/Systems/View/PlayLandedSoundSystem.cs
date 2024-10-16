using Leopotam.EcsLite;

namespace Scripts
{
    public class PlayLandedSoundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SoundService _soundService;
        private EcsFilter _movedFilter;
        private readonly EventListener _eventListener = new();

        private bool _hasEndedProcesses;

        public PlayLandedSoundSystem(SoundService soundService)
        {
            _soundService = soundService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _movedFilter = world.Filter<MoveProcess>().End();
            _movedFilter.AddEventListener(_eventListener);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var _ in _eventListener.OnRemove)
            {
                _hasEndedProcesses = true;
            }
            _eventListener.OnRemove.Clear();

            if (!_hasEndedProcesses) return;
            
            _soundService.PlaySound(SoundType.Landed);
            _hasEndedProcesses = false;
        }
    }
}