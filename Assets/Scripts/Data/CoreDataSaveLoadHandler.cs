using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CoreDataSaveLoadHandler : IInitializable, UnityEventHandler.IPause
    {
        private readonly SaveLoad _saveLoad;
        private readonly AppSessionData _appData;

        public CoreDataSaveLoadHandler(SaveLoad saveLoad, AppSessionData appData)
        {
            _saveLoad = saveLoad;
            _appData = appData;
            
        }

        public void Initialize()
        {
            this
                .ObserveEveryValueChanged(_ => _appData.State)
                .Pairwise()
                .Subscribe(p =>
                {
                    if (p is { Current: AppStates.Lobby, Previous: AppStates.Core })
                    {
                        WriteOrClearData();
                    }
                });
            
            TryLoadData();
        }
        
        public void Pause(bool pauseState)
        {
            if (pauseState)
            {
                WriteOrClearData();
            }
        }
        
        private void WriteOrClearData()
        {
            if (_appData.OngoingCoreSession == null)
                return;
            
            var isStatePlaying = _appData.OngoingCoreSession.CoreData.state == GameState.Playing;

            if (isStatePlaying )
            {
                WriteData();
            }
            else
            {
                ClearData();
            }
        }
        
        private void WriteData()
        {
            _appData.SavedCoreSession = _appData.OngoingCoreSession;
            _saveLoad.Save(_appData.SavedCoreSession.CoreData);
        }

        private void TryLoadData()
        {
            if (_saveLoad.IsExist<CoreData>())
            {
                var loadInfo = _saveLoad.Load<CoreData>();

                if (loadInfo.Result == LoadResult.Success)
                {
                    var loadedData = loadInfo.Obj;
                    _appData.SavedCoreSession = new CoreSessionData(loadedData);
                }
                else
                {
                    Debug.LogException(loadInfo.Exception);
                }
            }
        }

        private void ClearData()
        {
            _appData.SavedCoreSession = null;
            _saveLoad.Delete<CoreData>();
        }
    }
}