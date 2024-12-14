namespace LocalData
{
    using System.Collections.Generic;
    using GameCore.Services.Abstractions.LocalData;

    public class LevelLocalData : ILocalData
    {
        public Dictionary<int, LevelData> Levels { get; set; }
    }

    public class LevelData
    {
        public int   LevelId   { get; set; }
        public int   BestScore { get; set; }
        public float Accuracy  { get; set; }
        public int   BestCombo { get; set; }
    }
}