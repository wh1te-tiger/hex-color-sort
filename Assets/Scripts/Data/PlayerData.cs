using System;

namespace Scripts
{
    /// <summary>
    /// Serialized representation of player profile.
    /// </summary>
    [Serializable]
    public struct PlayerData
    {
        public int level;

        public PlayerData(int level)
        {
            this.level = level;
        }
    }
}