using Zenject;

namespace Scripts
{
    public class StartupHandler : IInitializable
    {
        private readonly AppSessionData _appData;
        private readonly CoreDataFactory _coreDataFactory;
        private readonly SceneService _sceneService;
        private readonly UiService _uiService;
        private readonly Signal _startGameSignal;

        public StartupHandler(AppSessionData appData, CoreDataFactory coreDataFactory, SceneService sceneService, UiService uiService, Signal signal)
        {
            _appData = appData;
            _coreDataFactory = coreDataFactory;
            _sceneService = sceneService;
            _uiService = uiService;
            _startGameSignal = signal;
        }
        
        public void Initialize()
        {
            _uiService.Initialize<LobbyWindow>();
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
            _appData.SavedCoreSession = coreData;
            _appData.HasFinishedCoreSession.Value = false;
            _sceneService.LoadScene(AppStates.Core);
        }
    }
}