using System;

namespace Scripts
{
    public class CoreSessionData
    {
        public CoreData CoreData { get; set; }

        public CoreSessionData(CoreData coreData)
        {
            CoreData = coreData;
        }
    }
    
    [Serializable]
    public struct CoreData
    {
        public int fieldId;
        public int score;
        public HexData[] hexes;
        public GameState state;

        public CoreData(int fieldId, int score, HexData[] hexes, GameState state)
        {
            this.fieldId = fieldId;
            this.score = score;
            this.hexes = hexes;
            this.state = state;
        }
    }

    [Serializable]
    public struct HexData
    {
        public ColorId color;
        public int index;
        public Coordinates pos;

        public HexData(ColorId color, int index, Coordinates pos)
        {
            this.color = color;
            this.index = index;
            this.pos = pos;
        }
    }
}