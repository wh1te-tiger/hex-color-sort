using System;
using Leopotam.EcsLite;

namespace Scripts
{
    public class LevelService : IDisposable
    {
        public int Score { get; private set; }
        public int WinScore { get; }
        
        private readonly EventListener _eventListener = new();

        public LevelService(EcsWorld world, LevelSettings levelSettings)
        {
            var collapsedFilter = world.Filter<CollapseRequest>().Exc<Delay>().End();
            collapsedFilter.AddEventListener(_eventListener);
            _eventListener.OnAdded += IncreaseScore;
            WinScore = levelSettings.WinCondition.Score;
        }
        
        private void IncreaseScore()
        {
            Score++;
            _eventListener.OnAdd.Clear();
        }
        
        #region Disposable

        public void Dispose()
        {
            _eventListener.OnAdded -= IncreaseScore;
        }

        #endregion
    }
}