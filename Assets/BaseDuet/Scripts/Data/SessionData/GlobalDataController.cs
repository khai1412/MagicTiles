namespace BaseDuet.Scripts.Data.SessionData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Data.LocalData;
    using BaseDuet.Scripts.Levels;
    using BaseDuet.Scripts.Models;
    using BaseDuet.Scripts.Signals;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Signals;
    using UnityEngine;

    public partial class GlobalDataController
    {
        private readonly GlobalData                  globalData;
        private readonly IAudioService               audioService;
        private readonly LevelController             levelController;
        private readonly SpeedBlueprint              speedBlueprint;
        private readonly BaseDuetLocalDataController duetLocalDataController;
        private readonly SignalBus                   signalBus;
        public           StaticValueBlueprint        StaticValueBlueprint { get; private set; }
        public GlobalDataController(
            GlobalData                  globalData,
            IAudioService               audioService,
            StaticValueBlueprint        staticValueBlueprint,
            LevelController             levelController,
            SpeedBlueprint              speedBlueprint,
            BaseDuetLocalDataController duetLocalDataController,
            SignalBus                   signalBus
        )
        {
            this.globalData              = globalData;
            this.audioService            = audioService;
            this.levelController         = levelController;
            this.speedBlueprint          = speedBlueprint;
            this.duetLocalDataController = duetLocalDataController;
            this.signalBus               = signalBus;
            this.StaticValueBlueprint    = staticValueBlueprint;
            this.signalBus.Subscribe<NoteHitSignal>(this.OnNoteHitSignal);
        }

        #region Properties

        private float TimeScale
        {
            get => Time.timeScale;
            set
            {
                Time.timeScale = value;
                try
                {
                    this.audioService.SetPlayListPitch(value);
                }
                catch (Exception e)
                {
                    Debug.LogError($"No sound to set pitch");
                }
            }
        }

        private float CurrentTimeScale
        {
            get => this.globalData.CurrentTimeScale;
            set
            {
                this.globalData.CurrentTimeScale = Mathf.Clamp(value, 0, this.HighestTimeScale);
                if (this.TimeScale != 0) this.TimeScale = this.globalData.CurrentTimeScale;
            }
        }

        public int    CurrentHealth { get => this.globalData.CurrentHealth; set => this.globalData.CurrentHealth = value; }
        public int    MaxHealth     { get => this.globalData.MaxHealth;     set => this.globalData.MaxHealth = value; }
        public string CurrentSongId { get => this.globalData.CurrentSongId; set => this.globalData.CurrentSongId = value; }

        public float CurrentSongScoreMultiplier { get => this.globalData.CurrentSongScoreMultiplier; set => this.globalData.CurrentSongScoreMultiplier = value; }

        public int Score { get => this.globalData.Score; set => this.globalData.Score = value; }

        public float        NoteSpeed             { get => this.globalData.CurrentNoteSpeed;      private set => this.globalData.CurrentNoteSpeed = value; }
        public ESongSegment CurrentSongSegment    { get => this.globalData.CurrentSongSegment;    set => this.globalData.CurrentSongSegment = value; }
        public string       CurrentSongDifficulty { get => this.globalData.CurrentSongDifficulty; set => this.globalData.CurrentSongDifficulty = value; }

        public int Streak { get => this.globalData.Streak; set => this.globalData.Streak = value; }

        public int MaxStreak { get => this.globalData.MaxStreak; set => this.globalData.MaxStreak = value; }

        public float Progress    { get => this.globalData.Progress; set => this.globalData.Progress = value; }
        public float Offlane     { get => this.duetLocalDataController.Offlane; }
        public int   ClaimedStar { get => this.globalData.ClaimedStar; set => this.globalData.ClaimedStar = value; }

        public bool IsCreativeMode
        {
            #if CREATIVE
            get => this.globalData.IsCreativeMode;
            set => this.globalData.IsCreativeMode = value;
            #else
            get => false;
            set { }
            #endif
        }

        public bool IsPlaying
        {
            get => this.globalData.IsPlaying;
            set
            {
                this.globalData.IsPlaying = value;
                Debug.Log($"Is playing: {value}");
            }
        }

        public bool  IsWin        { get => this.globalData.IsWin;        private set => this.globalData.IsWin = value; }
        public bool  IsInvincible { get => this.globalData.IsInvincible; set => this.globalData.IsInvincible = value; }
        public float MinX         { get => this.globalData.MinX; }
        public bool  IsCheating   { get => this.globalData.IsCheating; set => this.globalData.IsCheating = value; }

        public float MaxX               { get => this.globalData.MaxX; }
        public float Sensitivity        { get => this.duetLocalDataController.Sensitivity; }
        public float Latency            { get => this.duetLocalDataController.Latency; }
        public float NoteMargin         { get => this.duetLocalDataController.NoteMargin; }
        public int   CurrentReviveCount { get => this.globalData.CurrentReviveCount;              set => this.globalData.CurrentReviveCount = value; }
        public bool  IsGameplayTutorial { get => this.duetLocalDataController.IsGameplayTutorial; set => this.duetLocalDataController.IsGameplayTutorial = value; }
        public bool  IsCardTutorial     { get => this.duetLocalDataController.IsCardTutorial;     set => this.duetLocalDataController.IsCardTutorial = value; }
        public bool  IsObstacleTutorial { get => this.duetLocalDataController.IsObstacleTutorial; set => this.duetLocalDataController.IsObstacleTutorial = value; }
        public bool  IsShopTutorial     { get => this.duetLocalDataController.IsShopTutorial;     set => this.duetLocalDataController.IsShopTutorial = value; }
        public bool  IsSongTutorial     { get => this.duetLocalDataController.IsSongTutorial;     set => this.duetLocalDataController.IsSongTutorial = value; }
        public bool  IsAlbumTutorial    { get => this.duetLocalDataController.IsAlbumTutorial;    set => this.duetLocalDataController.IsAlbumTutorial = value; }

        #endregion

        #region Methods

        private CancellationTokenSource invicibleCancelToken;
        private void OnNoteHitSignal(NoteHitSignal signal)
        {
            if (signal.NoteModel.IsObstacle)
            {
                if (!this.IsInvincible && !this.IsGameplayTutorial) this.TotalObstacleHit++;
            }
            // else if (signal.NoteModel.IsHit && signal.NoteModel.ELongNote is not ELongNote.Body and not ELongNote.Tail)
            // {
            //     Debug.Log($"Note hit head");
            //     this.TotalNoteHit++;
            // }
            else if (signal.NoteModel.IsHit  && signal.NoteModel.ELongNote is not ELongNote.Body and not ELongNote.Tail)
            {
                Debug.Log($"Note hit ");

                this.TotalNoteHit++;
            }

            this.signalBus.Fire(new NoteDataChangeSignal());
        }

        public void PauseTime()  { this.TimeScale = 0; }
        public void ResumeTime() { this.TimeScale = this.CurrentTimeScale; }
        public void ResetTime()  { this.TimeScale = 1; }
        public void UpdateTimeScale(float value)
        {
            this.CurrentTimeScale = value;
            this.TimeScale        = this.CurrentTimeScale;
        }
        public void PlayGame()
        {
            this.CurrentReviveCount = 0;
            this.IsPlaying          = true;
            this.levelController.StartState();
        }
        public void Replaygame()
        {
            this.ResetNoteSpeed();
            this.IsPlaying = false;
            this.levelController.RestartState();
        }
        public void WinGame()  { this.IsWin = true; }
        public void LoseGame() { this.IsWin = false; }
        public void EndGame()
        {
            this.invicibleCancelToken?.Cancel();
            this.TimeScale = 1;
            this.IsPlaying = false;
            this.levelController.EndState();
        }

        public void SaveResult(int score, float maxAccuracy, int maxCombo, bool fullCombo, bool fullPerfect)
        {
            var songResult = new BaseDuetLocalDataRecord()
            {
                FullCombo   = fullCombo,
                FullPerfect = fullPerfect,
                HighScore   = score,
                MaxAccuracy = maxAccuracy,
                MaxCombo    = maxCombo,
                StarCount   = this.ClaimedStar
            };
            this.duetLocalDataController.FinishSong(this.CurrentSongId, songResult);
        }
        public bool                                        CheckFinishSong(string songName) => this.duetLocalDataController.CheckFinishSong(songName);
        public BaseDuetLocalDataRecord                     GetResultRecord(string songName) => this.duetLocalDataController.GetResultRecord(songName);
        public Dictionary<string, BaseDuetLocalDataRecord> GetAllResultRecord()             => this.duetLocalDataController.GetAllResultRecord();
        public void                                        ClaimCard(string songname)       { this.duetLocalDataController.ClaimCard(songname); }

        public void SetSensitivity(float value) => this.duetLocalDataController.SetSensitivity(value);
        public void SetMargin(float      value) => this.duetLocalDataController.SetMargin(value);
        public void SetLatency(float     value) => this.duetLocalDataController.SetLatency(value);
        public void SetOfflane(float     value) => this.duetLocalDataController.SetOfflane(value);
        public void Revive()
        {
            try
            {
                this.invicibleCancelToken = new CancellationTokenSource();
                this.CurrentReviveCount++;
                this.IsInvincible = true;
                var invincibleTime = this.InvincibleTime;
                DOTween.To(() => invincibleTime, value => invincibleTime = value, 0, invincibleTime).OnComplete(() =>
                       {
                           this.IsInvincible = false;
                       })
                       .ToUniTask(cancellationToken: this.invicibleCancelToken.Token);
            }
            catch (Exception e)
            {
                this.IsInvincible = false;
            }
        }
        public void NextSegment()
        {
            if (this.CurrentSongSegment == ESongSegment.Chorus)
                SetNoteSpeed(this.CurrentSongDifficulty, ESongSegment.Chorus);
            else
                SetNoteSpeed(this.CurrentSongDifficulty, (ESongSegment)((int)this.CurrentSongSegment + 1));
        }
        public void SetNoteSpeed(string difficulty, ESongSegment segment)
        {
            this.CurrentSongDifficulty = difficulty;
            this.CurrentSongSegment    = segment;
            this.NoteSpeed             = this.speedBlueprint[difficulty].SegmentSpeedRecords[segment].Speed;
            Debug.Log($"Note speed: {this.NoteSpeed}");
        }
        public void ResetNoteSpeed() => this.SetNoteSpeed(this.CurrentSongDifficulty, ESongSegment.Intro);

        #endregion
    }
}