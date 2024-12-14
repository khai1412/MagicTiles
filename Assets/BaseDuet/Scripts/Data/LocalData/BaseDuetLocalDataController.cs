namespace BaseDuet.Scripts.Data.LocalData
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class BaseDuetLocalDataController
    {
        private readonly BaseDuetLocalData baseDuetLocalData;
        public BaseDuetLocalDataController(BaseDuetLocalData baseDuetLocalData) { this.baseDuetLocalData = baseDuetLocalData; }

        #region Properties

        public float Sensitivity        { get => this.baseDuetLocalData.Sensitivity; }
        public float Latency            { get => this.baseDuetLocalData.Latency; }
        public float Offlane            { get => this.baseDuetLocalData.Offlane; }
        public float NoteMargin            { get => this.baseDuetLocalData.NoteMargin; }
        public bool  IsGameplayTutorial { get => this.baseDuetLocalData.IsGameplayTutorial; set => this.baseDuetLocalData.IsGameplayTutorial = value; }
        public bool  IsCardTutorial     { get => this.baseDuetLocalData.IsCardTutorial;     set => this.baseDuetLocalData.IsCardTutorial = value; }
        public bool  IsShopTutorial     { get => this.baseDuetLocalData.IsShopTutorial;     set => this.baseDuetLocalData.IsShopTutorial = value; }
        public bool  IsSongTutorial     { get => this.baseDuetLocalData.IsSongTutorial;     set => this.baseDuetLocalData.IsSongTutorial = value; }
        public bool  IsObstacleTutorial { get => this.baseDuetLocalData.IsObstacleTutorial; set => this.baseDuetLocalData.IsObstacleTutorial = value; }
        public bool  IsAlbumTutorial    { get => this.baseDuetLocalData.IsAlbumTutorial;    set => this.baseDuetLocalData.IsAlbumTutorial = value; }

        #endregion

        #region Methods

        public void FinishSong(string songId, BaseDuetLocalDataRecord record)
        {
            if (!this.baseDuetLocalData.LevelToRecord.TryGetValue(songId, out _)) this.baseDuetLocalData.LevelToRecord.Add(songId, record);
            else this.UpdateSong(songId, record);
        }

        public bool CheckFinishSong(string songName) => this.baseDuetLocalData.LevelToRecord.ContainsKey(songName);
        public void UpdateSong(string songId, BaseDuetLocalDataRecord record)
        {
            if (this.baseDuetLocalData.LevelToRecord[songId].HighScore > record.HighScore) return;
            this.baseDuetLocalData.LevelToRecord[songId].HighScore   = Mathf.Max(this.baseDuetLocalData.LevelToRecord[songId].HighScore, record.HighScore);
            this.baseDuetLocalData.LevelToRecord[songId].StarCount   = Mathf.Max(this.baseDuetLocalData.LevelToRecord[songId].StarCount, record.StarCount);
            this.baseDuetLocalData.LevelToRecord[songId].MaxCombo    = Mathf.Max(this.baseDuetLocalData.LevelToRecord[songId].MaxCombo, record.MaxCombo);
            this.baseDuetLocalData.LevelToRecord[songId].FullCombo   = record.FullCombo || this.baseDuetLocalData.LevelToRecord[songId].FullCombo;
            this.baseDuetLocalData.LevelToRecord[songId].FullPerfect = record.FullPerfect || this.baseDuetLocalData.LevelToRecord[songId].FullPerfect;
            this.baseDuetLocalData.LevelToRecord[songId].MaxAccuracy = Mathf.Max(this.baseDuetLocalData.LevelToRecord[songId].MaxAccuracy, record.MaxAccuracy);
        }
        public BaseDuetLocalDataRecord GetResultRecord(string songName)
        {
            BaseDuetLocalDataRecord record;
            this.baseDuetLocalData.LevelToRecord.TryGetValue(songName, out record);
            return record;
        }
        public Dictionary<string, BaseDuetLocalDataRecord> GetAllResultRecord() => this.baseDuetLocalData.LevelToRecord;
        public void ClaimCard(string songName)
        {
            BaseDuetLocalDataRecord record;
            if (this.baseDuetLocalData.LevelToRecord.TryGetValue(songName, out record))
            {
                record.ClaimedReward = true;
            }
        }
        public void SetSensitivity(float value) => this.baseDuetLocalData.Sensitivity = value;
        public void SetMargin(float value) => this.baseDuetLocalData.NoteMargin = value;
        public void SetLatency(float     value) => this.baseDuetLocalData.Latency = value;
        public void SetOfflane(float     value) => this.baseDuetLocalData.Offlane = value;
        public void FinishTutorial()            => this.baseDuetLocalData.IsGameplayTutorial = false;

        #endregion
    }
}