using Leopotam.EcsLite;

namespace Scripts
{
    public class CreateLobbyUiSystem : IEcsInitSystem
    {
        private readonly UiService _uiService;

        public CreateLobbyUiSystem(UiService uiService)
        {
            _uiService = uiService;
        }

        public void Init(IEcsSystems systems)
        {
            _uiService.Initialize<LobbyWindow>();
        }
    }
}