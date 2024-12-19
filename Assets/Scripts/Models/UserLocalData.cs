namespace MagicTiles.Scripts.Models
{
    using System;
    using System.Collections.Generic;
    using GameCore.Services.Abstractions.LocalData;

    public class UserLocalData : ILocalData
    {
        public Dictionary<string, int>              AccumulatedDictionary = new();
        public Dictionary<string, DogDuetLevelData> DictLevelData         = new();
        public bool                                 IsFirstOpen           = true;

        public Type ControllerType => typeof(UserLocalDataController);
        public void Init()         { }
    }

    public class DogDuetLevelData
    {
        public bool        HasPassed;
        public bool        HasWatchAdUnlock;
        public LevelStatus LevelStatus;
        public int         Story;
    }
}