namespace MagicTiles.Scripts.Models
{
    using System;
    using System.Collections.Generic;
    using GameCore.Services.Abstractions.LocalData;

    public class UserLocalData : ILocalData
    {
        public bool                                 IsFirstOpen           = true;
        public Dictionary<string, DogDuetLevelData> DictLevelData         = new();
        public Dictionary<string, int>              AccumulatedDictionary = new();
        public void                                 Init() { }

        public Type ControllerType => typeof(UserLocalDataController);
    }

    public class DogDuetLevelData
    {
        public int         Story;
        public LevelStatus LevelStatus;
        public bool        HasWatchAdUnlock;
        public bool        HasPassed;
    }
}