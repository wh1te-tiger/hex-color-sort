using Leopotam.EcsLite;
using Zenject;

namespace Scripts
{
    public class CreateUiSystem : IEcsInitSystem
    {
        [Inject] private UiService _uiService;
        
        public void Init(IEcsSystems systems)
        {
            _uiService.Initialize<CoreWindow>();
        }
    }
}