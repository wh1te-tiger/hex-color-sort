using UnityEngine;

namespace Scripts
{
    public class PlayerDataSaveLoadHandler : UnityEventHandler.IPause, UnityEventHandler.IQuit
    {
        private readonly SaveLoad _saveLoad;
        private readonly AppSessionData _data;

        public PlayerDataSaveLoadHandler(SaveLoad saveLoad, AppSessionData data)
        {
            _saveLoad = saveLoad;
            _data = data;
            
            LoadPlayer();
        }

        public void Pause(bool pause)
        {
            if (pause) 
                SavePlayer();
        }

        public void Quit()
        {
            SavePlayer();
        }

        private void SavePlayer()
        {
            var playerData = new PlayerData(_data.NextLevelID);
            _saveLoad.Save(playerData);
        }

        private void LoadPlayer()
        {
            if (!_saveLoad.IsExist<PlayerData>()) return;
            
            var loadInfo = _saveLoad.Load<PlayerData>();
            if (loadInfo.Result == LoadResult.Success)
            {
                var data = loadInfo.Obj;
                Load(data);
            }
            else
            {
                Debug.LogException(loadInfo.Exception);
            }
        }

        private void Load(PlayerData data)
        {
            _data.NextLevelID = data.level;
        }
    }
}