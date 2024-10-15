using System;
using Zenject;

namespace Scripts
{
    public class FinalizeService : IInitializable, IDisposable
    {
        private readonly SceneService _sceneService;
        private readonly Signal _returnToLobbyRequest;
        
        public FinalizeService(Signal returnToLobbyRequest, SceneService sceneService)
        {
            _returnToLobbyRequest = returnToLobbyRequest;
            _sceneService = sceneService;
        }

        public void Initialize()
        {
            _returnToLobbyRequest.Subscribe(OnGameEnded);
        }

        private void OnGameEnded()
        {
            _sceneService.LoadScene(AppStates.Lobby);
        }

        public void Dispose()
        {
            _returnToLobbyRequest?.Dispose();
        }
    }
}