using Leopotam.EcsLite;
using Zenject;

namespace Scripts
{
    public class StartupHandler : IInitializable
    {
        private readonly AppSessionData _appData;
        private readonly CoreDataFactory _coreDataFactory;
        private readonly SceneService _sceneService;
        private readonly Signal _startGameSignal;
        private readonly EcsWorld _world;

        public StartupHandler(AppSessionData appData, CoreDataFactory coreDataFactory, SceneService sceneService, Signal signal, EcsWorld world)
        {
            _appData = appData;
            _coreDataFactory = coreDataFactory;
            _sceneService = sceneService;
            _startGameSignal = signal;
            _world = world;
        }
        
        public void Initialize()
        {
            if (_appData.HasSavedCoreSession)
            {
                _startGameSignal.Subscribe(() => LoadCore(_appData.SavedCoreSession)) ;
            }
            else
            {
                _startGameSignal.Subscribe(() => LoadCore(_coreDataFactory.Create()));
            }
        }

        private void LoadCore(CoreSessionData coreData)
        {
            _world.Destroy();
            
            _appData.OngoingCoreSession = coreData;
            _appData.HasFinishedCoreSession.Value = false;
            _sceneService.LoadScene(AppStates.Core);
        }
    }
}