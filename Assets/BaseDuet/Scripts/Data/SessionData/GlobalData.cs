namespace BaseDuet.Scripts.Data.SessionData
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Models;
    using UnityEngine;

    public class GlobalData
    {
        public float CurrentTimeScale { get; set; }

        public string       CurrentSongId              { get; set; }
        public float        CurrentSongScoreMultiplier { get; set; }
        public float        CurrentNoteSpeed           { get; set; }
        public ESongSegment CurrentSongSegment         { get; set; }
        public string       CurrentSongDifficulty      { get; set; }

        public int CurrentHealth { get; set; }
        public int MaxHealth     { get; set; }

        public int   Score     { get; set; }
        public int   Streak    { get; set; }
        public int   MaxStreak { get; set; }
        public float Progress  { get; set; }

        public bool  IsCreativeMode     { get; set; } = false;
        public bool  IsPlaying          { get; set; }
        public bool  IsWin              { get; set; }
        public bool  IsInvincible       { get; set; }
        public int   ClaimedStar        { get; set; }
        public float MinX               { get; set; } = .42f;
        public float MaxX               { get; set; } = 1.3f;
        public int   CurrentReviveCount { get; set; } = 0;
        public bool  IsCheating         { get; set; } = false;
    }
}