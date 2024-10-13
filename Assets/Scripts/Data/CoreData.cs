using System;

namespace Scripts
{
    public class CoreSessionData
    {
        public CoreData CoreData { get; }

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

        public CoreData(int fieldId, int score, HexData[] hexes)
        {
            this.fieldId = fieldId;
            this.score = score;
            this.hexes = hexes;
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