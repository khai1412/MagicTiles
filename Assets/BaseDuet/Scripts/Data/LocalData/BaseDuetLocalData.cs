namespace BaseDuet.Scripts.Data.LocalData
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using Sirenix.Serialization;

    public class BaseDuetLocalData : ILocalData
    {
        public void Init() { }

        [OdinSerialize] public Dictionary<string, BaseDuetLocalDataRecord> LevelToRecord      { get; set; } = new();
        [OdinSerialize] public float                                       Sensitivity        { get; set; } = 2;
        [OdinSerialize] public float                                       Latency            { get; set; } = 0;
        [OdinSerialize] public float                                       Offlane            { get; set; } = 0.18f;
        [OdinSerialize] public float                                       NoteMargin            { get; set; } = 1f;
        [OdinSerialize] public bool                                        IsGameplayTutorial { get; set; } = false;
        [OdinSerialize] public bool                                        IsCardTutorial     { get; set; } = false;
        [OdinSerialize] public bool                                        IsSongTutorial     { get; set; } = false;
        [OdinSerialize] public bool                                        IsShopTutorial     { get; set; } = false;
        [OdinSerialize] public bool                                        IsObstacleTutorial { get; set; } = false;
        [OdinSerialize] public bool                                        IsAlbumTutorial    { get; set; } = false;
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