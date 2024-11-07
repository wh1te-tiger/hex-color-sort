using System;
using UniRx;

namespace Scripts
{
    public class AppSessionData
    {
        public int NextLevelID { get; set; }
        public CoreSessionData SavedCoreSession;
        public bool HasSavedCoreSession => SavedCoreSession != null;
        public readonly BoolReactiveProperty IsLastCoreSessionWon = new();
        public readonly BoolReactiveProperty HasFinishedCoreSession = new();
        public CoreSessionData OngoingCoreSession;
        public AppStates State;
        
        public readonly LevelSettings[] Levels;
        
        public AppSessionData(LevelSettings[] levels)
        {
            Levels = levels;
            NextLevelID = 1;
            HasFinishedCoreSession
                .Where(v => v)
                .Subscribe(_ =>
                {
                    if (IsLastCoreSessionWon.Value)
                    {
                        NextLevelID++;
                    }
                });
        }

        public LevelSettings GetLevel()
        {
            foreach (var level in Levels)
            {
                if (level.Id == NextLevelID)
                {
                    return level;
                }
            }
#if UNITY_EDITOR
            throw new Exception($"No level with the id: {NextLevelID} is found");
#else
        return Levels[(NextLevelID + 1) % Levels.Length];
#endif
        }
    }
}