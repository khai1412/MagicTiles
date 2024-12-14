namespace TheOneStudio.HyperCasual.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BaseDuet.Scripts.Characters;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Levels;
    using BaseDuet.Scripts.Models;
    using BaseDuet.Scripts.Signals;
    using Cysharp.Threading.Tasks;;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Helpers;
    using TheOneStudio.HyperCasual.MIDI;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Models.StaticModel;
    using TheOneStudio.HyperCasual.StateMachines.Game;
    using TheOneStudio.HyperCasual.StateMachines.Game.States;
    using TheOneStudio.HyperCasual.Utils;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using UnityEngine;


    public class SongManager
    {
        #region Inject

        private readonly LevelController                   levelController;
        private readonly UITemplateSettingDataController   uiTemplateSettingDataController;
        private readonly GlobalDataController              globalDataController;
        private readonly MidiGenerator                     midiGenerator;
        private readonly LevelBlueprint                    levelBlueprint;
        private readonly IDependencyContainer              diContainer;
        private readonly UserLocalDataController           userLocalDataController;
        private readonly TimeHelper                        timeHelper;
        private readonly AmaGDKAnalyticsHelper             amaGdkAnalyticsHelper;
        private readonly SignalBus                         signalBus;
        private readonly WebRequestUtils                   webRequestUtils;
        private readonly IGameAssets                       gameAssets;
        private readonly IAudioService                     audioService;
        private readonly UITemplateInventoryDataController uiTemplateInventoryDataController;
        private readonly PreviewSongManager                previewSongManager;


        public SongManager(LevelController                   levelController,      UITemplateSettingDataController uiTemplateSettingDataController,
                           GlobalDataController              globalDataController, MidiGenerator                   midiGenerator, LevelBlueprint levelBlueprint,
                           IDependencyContainer              diContainer,
                           UserLocalDataController           userLocalDataController,
                           TimeHelper                        timeHelper,
                           AmaGDKAnalyticsHelper             amaGdkAnalyticsHelper,
                           SignalBus                         signalBus,
                           WebRequestUtils                   webRequestUtils,
                           IGameAssets                       gameAssets,
                           IAudioService                     audioService,
                           UITemplateInventoryDataController uiTemplateInventoryDataController,
                           PreviewSongManager                previewSongManager
        )
        {
            this.levelController                   = levelController;
            this.uiTemplateSettingDataController   = uiTemplateSettingDataController;
            this.globalDataController              = globalDataController;
            this.midiGenerator                     = midiGenerator;
            this.levelBlueprint                    = levelBlueprint;
            this.diContainer                       = diContainer;
            this.userLocalDataController           = userLocalDataController;
            this.timeHelper                        = timeHelper;
            this.amaGdkAnalyticsHelper             = amaGdkAnalyticsHelper;
            this.signalBus                         = signalBus;
            this.webRequestUtils                   = webRequestUtils;
            this.gameAssets                        = gameAssets;
            this.audioService                      = audioService;
            this.uiTemplateInventoryDataController = uiTemplateInventoryDataController;
            this.previewSongManager                = previewSongManager;
            this.InitSong("");
            this.signalBus.Subscribe<DuetLevelChangeStateSignal>(this.OnDuetLevelChangeState);
            this.signalBus.Subscribe<NoteHitSignal>(this.OnNoteHitSignal);
        }

        #endregion

        #region Properties

        public  LevelModel            CurrentLevelModel                { get; private set; }
        public  DuetComposedLevelData currentDuetDuetComposedLevelData { get; private set; }
        public  List<LevelRecord>     ListLevelRecord                  { get; set; }
        private EPlayType             lastPlayType;

        public EPlayType LastPlayType
        {
            get
            {
                if (this.globalDataController.IsGameplayTutorial) return EPlayType.Tutorial;
                return this.lastPlayType;
            }
            set { this.lastPlayType = value; }
        }

        private bool hasLogMeStart = false;

        #endregion

        private void OnDuetLevelChangeState(DuetLevelChangeStateSignal signal)
        {
            switch (signal.state)
            {
                case EDuetLevelState.StartState:
                    this.hasLogMeStart = false;

                    this.amaGdkAnalyticsHelper.LogEvent(EventName.SongStart, new AmaGDKLogModel()
                    {
                        AcmId        = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                        SongName     = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                        SongPlayType = this.LastPlayType.ToString()
                    });
                    this.timeHelper.StartSong();
                    break;
                case EDuetLevelState.LoseState:
                    this.amaGdkAnalyticsHelper.LogEvent(EventName.SongFail, new AmaGDKLogModel()
                    {
                        AcmId        = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                        SongName     = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                        SongPlayTime = (int)this.audioService.GetPlayListTime()
                    });


                    break;
                case EDuetLevelState.EndState:
                    this.timeHelper.EndSong();
                    break;
                case EDuetLevelState.RestartState:
                    this.amaGdkAnalyticsHelper.LogEvent(EventName.SongClick, new AmaGDKLogModel()
                    {
                        AcmId          = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                        SongName       = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                        SongPlayType   = EPlayType.Replay.ToString(),
                        SongUnlockType = EUnlockType.Default.ToString()
                    });
                    break;
            }
        }
        private void OnNoteHitSignal(NoteHitSignal signal)
        {
            if (!this.hasLogMeStart && signal.NoteModel.TimeAppear >= 15 && signal.NoteModel.TimeAppear <= 16) this.LogMeStartOnce();
            // if (Mathf.Abs(signal.noteModel.Process - 1) < 0.000001f)
            //     UniTask.WaitUntil(() => Math.Abs(this.audioService.GetPlayListTime() - 1) < 0.01f).ContinueWith(() =>
            //     {
            //         this.audioService.StopAllPlayList();
            //     });
        }
        private void LogMeStartOnce()
        {
            Debug.Log("Log me start");
            this.hasLogMeStart = true;
            this.amaGdkAnalyticsHelper.LogEvent(EventName.MeStart, new AmaGDKLogModel()
            {
                AcmId        = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                SongName     = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                SongPlayType = this.LastPlayType.ToString()
            });
        }
        public void UpdateNoteVolume() { this.levelController.UpdateVolume(this.uiTemplateSettingDataController.SoundValue); }

        public void SelectSong(DuetComposedLevelData duetComposedLevelData)
        {
            this.globalDataController.CurrentSongId = duetComposedLevelData.LevelRecord.SongId;
            this.currentDuetDuetComposedLevelData   = duetComposedLevelData;
        }

        public async UniTask SelectDefaultSong(bool playSong = true)
        {
            var levelData = this.GetAllLevelData().OrderBy(x => x.Value.RemoteLevelRecord.Story).FirstOrDefault(x => !x.Value.DogDuetLevelData.HasPassed).Value;
            levelData ??= this.GetAllLevelData().OrderBy(x => x.Value.LevelRecord.Story).First().Value;

            this.SelectSong(levelData);
            if (playSong)
            {
                this.previewSongManager.PlayPreviewSong(levelData.RemoteLevelRecord.previewAudioUrl);
            }
        }

        public void SelectTutorialSong()
        {
            var tutorialRecord = new LevelRecord
            {
                ListThemes = new() { "BlueTheme" },
                SongId     = StaticStrings.TutorialSongId,
            };

            var remoteTutorialRecord = new RemoteLevelRecord
            {
                songName = StaticTutorialSong.Song
            };

            var tutorialLevelData = new DogDuetLevelData();

            this.globalDataController.CurrentSongId = StaticStrings.TutorialSongId;
            this.currentDuetDuetComposedLevelData   = new(tutorialLevelData, tutorialRecord, remoteTutorialRecord, null);
            this.PlayCurrentSong(EPlayType.Tutorial, redirectToPrepareState: false);
        }

        public void PlaySong(DuetComposedLevelData duetComposedLevelData, EPlayType ePlayType = EPlayType.Home, EUnlockType eUnlockType = EUnlockType.Default)
        {
            this.SelectSong(duetComposedLevelData);
            this.PlayCurrentSong(ePlayType, eUnlockType);
        }
        public void PlayCurrentSong(EPlayType ePlayType = EPlayType.Home, EUnlockType eUnlockType = EUnlockType.Default, bool redirectToPrepareState = true)
        {
            this.LastPlayType = this.globalDataController.IsGameplayTutorial ? EPlayType.Tutorial : ePlayType;
            this.amaGdkAnalyticsHelper.LogEvent(EventName.SongClick, new AmaGDKLogModel()
            {
                AcmId          = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                SongName       = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                SongPlayType   = this.LastPlayType.ToString(),
                SongUnlockType = eUnlockType.ToString()
            });
            if (redirectToPrepareState) this.diContainer.Resolve<GameStateMachine>().TransitionTo<GamePrepareState>();
            this.hasLogMeStart = false;
        }

        public async UniTask ReplayCurrentSong()
        {
            this.LastPlayType = EPlayType.Replay;
            this.amaGdkAnalyticsHelper.LogEvent(EventName.SongClick, new AmaGDKLogModel()
            {
                AcmId          = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                SongName       = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                SongPlayType   = EPlayType.Replay.ToString(),
                SongUnlockType = EUnlockType.Default.ToString()
            });

            this.diContainer.Resolve<GameStateMachine>().TransitionTo<GamePrepareState>();
        }
        public async UniTask LoadSong(byte[] midiContent, AudioClip audioClip, int seed = 0)
        {
            var models = await this.midiGenerator.GetNoteModels(midiContent, seed, this.currentDuetDuetComposedLevelData.RemoteLevelRecord.difficultyLevel,
                                                                this.currentDuetDuetComposedLevelData.LevelRecord.HasObstacle);
            this.globalDataController.TotalNote     = models.Count(x => !x.IsObstacle && x.ELongNote is not (ELongNote.Body or ELongNote.Tail));
            Debug.Log($"Total note: {this.globalDataController.TotalNote}");
            this.globalDataController.TotalObstacle = models.Count(x => x.IsObstacle);
            var moodThemes = this.currentDuetDuetComposedLevelData.LevelRecord.ListThemes
                                 .Select(x => this.gameAssets.LoadAssetAsync<MoodThemeConfig>(x).WaitForCompletion()).ToList();

            var levelModel = new LevelModel(models, this.currentDuetDuetComposedLevelData.LevelRecord.StoryIntro,
                                            this.currentDuetDuetComposedLevelData.LevelRecord.StoryOutro, moodThemes,
                                            new CharacterModel(this.globalDataController.CharacterSpeed, this.globalDataController.CharacterPositionY),
                                            new CharacterModel(this.globalDataController.CharacterSpeed, this.globalDataController.CharacterPositionY));
            this.CurrentLevelModel = levelModel;
            this.levelController.BindData(this.CurrentLevelModel, null);
            await this.levelController.PrepareMusic(audioClip);
        }
        public void InitSong(string strListSong)
        {
            if (String.IsNullOrEmpty(strListSong)) this.ListLevelRecord = this.levelBlueprint.Select(x => x.Value).ToList();
            else
            {
                var listSong = strListSong.Split(",").Select(x => x).ToList();
                this.ListLevelRecord = this.levelBlueprint.Where(x => listSong.Contains(x.Value.SongId)).Select(x => x.Value).ToList();
            }
        }
        public Dictionary<string, DuetComposedLevelData> GetAllLevelData()                 { return this.userLocalDataController.GetDictionaryLeveData(this.globalDataController); }
        public DuetComposedLevelData                     GetLevelRecordByCard(string card) { return this.userLocalDataController.GetDictionaryLeveData(this.globalDataController).FirstOrDefault(x => x.Value.LevelRecord.RewardCard == card).Value; }
        public DuetComposedLevelData GetNextSong()
        {
            return this.userLocalDataController.GetDictionaryLeveData(this.globalDataController)
                       .FirstOrDefault(x => x.Value.RemoteLevelRecord.Story == this.currentDuetDuetComposedLevelData.RemoteLevelRecord.Story + 1).Value;
        }
        public void ClaimCard(LevelRecord levelRecord)
        {
            this.uiTemplateInventoryDataController.SetOwnedItemData(this.uiTemplateInventoryDataController.GetItemData(levelRecord.RewardCard));
            this.globalDataController.ClaimCard(levelRecord.SongId);
        }
        public void UnlockNextSong()
        {
            var nextSong = this.userLocalDataController.GetDictionaryLeveData(this.globalDataController).Values
                               .FirstOrDefault(x => x.LevelRecord.Story == this.currentDuetDuetComposedLevelData.LevelRecord.Story + 1);
            if (nextSong != null) nextSong.DogDuetLevelData.LevelStatus = LevelStatus.Unlocked;
        }

        public void PassCurrentSong()
        {
            this.amaGdkAnalyticsHelper.LogEvent(EventName.SongEnd, new AmaGDKLogModel()
            {
                AcmId    = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                SongName = this.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName
            });
            this.userLocalDataController.FinishSong(this.currentDuetDuetComposedLevelData.LevelRecord.SongId);
        }
        public int GetMaxSongResult(ESongMode songMode)
        {
            return songMode.GetPointPerFood() * this.globalDataController.TotalNote
                   + songMode.GetPointPerObstacle() * this.globalDataController.TotalObstacle;
        }
        public int GetFinalSongResult(ESongMode songMode)
        {
            return songMode.GetPointPerFood() * this.globalDataController.TotalNoteHit
                   + songMode.GetPointPerObstacle() * this.globalDataController.TotalObstaclePassed
                   - songMode.GetMinusPointPerDead() * this.globalDataController.TotalObstacleHit;
        }
    }
}