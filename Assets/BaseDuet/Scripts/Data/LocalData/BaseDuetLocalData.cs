namespace BaseDuet.Scripts.Data.LocalData
{
    using System.Collections.Generic;
    using GameCore.Services.Abstractions.LocalData;

    public class BaseDuetLocalData : ILocalData
    {
        public void Init() { }

        public Dictionary<string, BaseDuetLocalDataRecord> LevelToRecord      { get; set; } = new();
        public float                                       Sensitivity        { get; set; } = 2;
        public float                                       Latency            { get; set; } = 0;
        public float                                       Offlane            { get; set; } = 0.18f;
        public float                                       NoteMargin         { get; set; } = 1f;
        public bool                                        IsGameplayTutorial { get; set; } = false;
        public bool                                        IsCardTutorial     { get; set; } = false;
        public bool                                        IsSongTutorial     { get; set; } = false;
        public bool                                        IsShopTutorial     { get; set; } = false;
        public bool                                        IsObstacleTutorial { get; set; } = false;
        public bool                                        IsAlbumTutorial    { get; set; } = false;
    }

    public class BaseDuetLocalDataRecord
    {
        public int   HighScore     { get; set; }
        public int   StarCount     { get; set; }
        public int   MaxCombo      { get; set; }
        public bool  FullCombo     { get; set; }
        public bool  FullPerfect   { get; set; }
        public float MaxAccuracy   { get; set; }
        public bool  ClaimedReward { get; set; }
    }
}