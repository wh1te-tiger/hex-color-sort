using UniRx;

namespace Scripts
{
    public class LevelService 
    {
        public int Score { get; private set; }
        public int WinScore { get; }
        public int LevelId { get; }
        public readonly ReadOnlyReactiveProperty<GameState> LevelState;

        private readonly AppSessionData _appSessionData;
        private GameState _state;
        
        public LevelService(LevelSettings levelSettings, AppSessionData appSessionData)
        {
            _appSessionData = appSessionData;
            WinScore = levelSettings.WinCondition.Score;
            var stateStream = this.ObserveEveryValueChanged(_ => _state);
            LevelState = new ReadOnlyReactiveProperty<GameState>(stateStream);
            Score = _appSessionData.OngoingCoreSession.CoreData.score;
            LevelId = _appSessionData.NextLevelID;
        }
        
        public void IncreaseScore()
        {
            Score++;
            
            if (Score >= WinScore)
            {
                _state = GameState.Win;
                _appSessionData.IsLastCoreSessionWon.Value = true;
                _appSessionData.HasFinishedCoreSession.Value = true;
            }
        }

        public void SetFailedState()
        {
            _state = GameState.Failed;
            _appSessionData.IsLastCoreSessionWon.Value = false;
            _appSessionData.HasFinishedCoreSession.Value = true;
        }
    }

    public enum GameState
    {
        Playing,
        Win,
        Failed
    }
}