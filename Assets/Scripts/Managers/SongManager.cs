namespace MagicTiles.Scripts.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using BaseDuet.Scripts.Characters;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Levels;
    using BaseDuet.Scripts.Models;
    using BaseDuet.Scripts.Signals;
    using Cysharp.Threading.Tasks;
    using GameCore.Core.AssetsManager;
    using GameCore.Core.AudioManager;
    using MagicTiles.Scripts.Blueprints;
    using MagicTiles.Scripts.Helpers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.Models;
    using MagicTiles.Scripts.StateMachines.Game;
    using MagicTiles.Scripts.StateMachines.Game.States;
    using MagicTiles.Scripts.Utils;
    using UnityEngine;

    public class SongManager
    {
        private void OnDuetLevelChangeState(DuetLevelChangeStateSignal signal)
        {
            switch (signal.state)
            {
                case EDuetLevelState.StartState:
                    this.hasLogMeStart = false;
                    this.timeHelper.StartSong();
                    break;
                case EDuetLevelState.LoseState:    break;
                case EDuetLevelState.EndState:     this.timeHelper.EndSong(); break;
                case EDuetLevelState.RestartState: break;
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
        }

        public void UpdateNoteVolume()
        {
            // this.levelController.UpdateVolume(this.uiTemplateSettingDataController.SoundValue);
        }

        public void SelectSong(DuetComposedLevelData duetComposedLevelData)
        {
            this.globalDataController.CurrentSongId = duetComposedLevelData.LevelRecord.SongId;
            this.currentDuetDuetComposedLevelData   = duetComposedLevelData;
        }

        public void SelectSong(string songId)
        {
            this.SelectSong(this.GetAllLevelData()[songId]);
        }

        public async UniTask SelectDefaultSong(bool playSong = true)
        {
            var levelData = this.GetAllLevelData().OrderBy(x => x.Value.RemoteLevelRecord.Story).FirstOrDefault(x => !x.Value.DogDuetLevelData.HasPassed).Value;
            levelData ??= this.GetAllLevelData().OrderBy(x => x.Value.LevelRecord.Story).First().Value;

            this.SelectSong(levelData);
            if (playSong) this.previewSongManager.PlayPreviewSong(levelData.RemoteLevelRecord.previewAudioUrl);
        }

        public void PlaySong(DuetComposedLevelData duetComposedLevelData, EPlayType ePlayType = EPlayType.Home, EUnlockType eUnlockType = EUnlockType.Default)
        {
            this.SelectSong(duetComposedLevelData);
            this.PlayCurrentSong(ePlayType, eUnlockType);
        }

        public void PlaySong(string songId)
        {
            this.SelectSong(songId);
            this.PlayCurrentSong();
        }

        public void PlayCurrentSong(EPlayType ePlayType = EPlayType.Home, EUnlockType eUnlockType = EUnlockType.Default, bool redirectToPrepareState = true)
        {
            this.LastPlayType = this.globalDataController.IsGameplayTutorial ? EPlayType.Tutorial : ePlayType;
            if (redirectToPrepareState) this.gameStateMachine.TransitionTo<GamePrepareState>();
            this.hasLogMeStart = false;
        }

        public async UniTask ReplayCurrentSong()
        {
            this.gameStateMachine.TransitionTo<GamePrepareState>();
        }

        public async UniTask LoadSong(byte[] midiContent, AudioClip audioClip, int seed = 0)
        {
            var songModel = this.GetAllLevelData()[this.globalDataController.CurrentSongId];
            var models    = await this.midiGenerator.GetNoteModels(midiContent, seed, songModel.RemoteLevelRecord.difficultyLevel, songModel.LevelRecord.HasObstacle);
            this.globalDataController.TotalNote = models.Count(x => !x.IsObstacle && x.ELongNote is not (ELongNote.Body or ELongNote.Tail));
            Debug.Log($"Total note: {this.globalDataController.TotalNote}");
            this.globalDataController.TotalObstacle = models.Count(x => x.IsObstacle);
            var moodThemes = songModel.LevelRecord.ListThemes.Select(x => this.gameAssets.Load<MoodThemeConfig>(x)).ToList();

            var levelModel = new LevelModel(models, moodThemes, new CharacterModel(this.globalDataController.CharacterSpeed, this.globalDataController.CharacterPositionY));
            this.CurrentLevelModel = levelModel;
            this.levelController.BindData(this.CurrentLevelModel, null);
            await this.levelController.PrepareMusic(audioClip);
        }

        public void InitSong(string strListSong)
        {
            if (string.IsNullOrEmpty(strListSong))
            {
                this.ListLevelRecord = this.levelBlueprint.LevelRecords.Select(x => x.Value).ToList();
            }
            else
            {
                var listSong = strListSong.Split(",").Select(x => x).ToList();
                this.ListLevelRecord = this.levelBlueprint.LevelRecords.Where(x => listSong.Contains(x.Value.SongId)).Select(x => x.Value).ToList();
            }
        }

        public Dictionary<string, DuetComposedLevelData> GetAllLevelData()
        {
            return this.userLocalDataController.GetDictionaryLeveData(this.globalDataController);
        }

        public DuetComposedLevelData GetNextSong()
        {
            return this.userLocalDataController.GetDictionaryLeveData(this.globalDataController)
                .FirstOrDefault(x => x.Value.RemoteLevelRecord.Story == this.currentDuetDuetComposedLevelData.RemoteLevelRecord.Story + 1).Value;
        }

        public void ClaimCard(LevelRecord levelRecord)
        {
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

        #region Inject

        private readonly LevelController levelController;

        // private readonly UITemplateSettingDataController   uiTemplateSettingDataController;
        private readonly GlobalDataController globalDataController;

        private readonly MidiGenerator midiGenerator;

        private readonly LevelBlueprint          levelBlueprint;
        private readonly UserLocalDataController userLocalDataController;
        private readonly TimeHelper              timeHelper;
        private readonly WebRequestUtils         webRequestUtils;
        private readonly IAssetManager           gameAssets;
        private readonly IAudioManager           audioService;
        private readonly PreviewSongManager      previewSongManager;
        private readonly GameStateMachine        gameStateMachine;

        public SongManager(
            LevelController levelController,
            // UITemplateSettingDataController   uiTemplateSettingDataController,
            GlobalDataController    globalDataController,
            MidiGenerator           midiGenerator,
            LevelBlueprint          levelBlueprint,
            UserLocalDataController userLocalDataController,
            TimeHelper              timeHelper,
            WebRequestUtils         webRequestUtils,
            IAssetManager           gameAssets,
            IAudioManager           audioService,
            PreviewSongManager      previewSongManager
            // GameStateMachine        gameStateMachine
        )
        {
            this.levelController = levelController;
            // this.uiTemplateSettingDataController   = uiTemplateSettingDataController;
            this.globalDataController    = globalDataController;
            this.midiGenerator           = midiGenerator;
            this.levelBlueprint          = levelBlueprint;
            this.userLocalDataController = userLocalDataController;
            this.timeHelper              = timeHelper;
            this.webRequestUtils         = webRequestUtils;
            this.gameAssets              = gameAssets;
            this.audioService            = audioService;
            this.previewSongManager      = previewSongManager;
            // this.gameStateMachine        = gameStateMachine;
            this.InitSong("");
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
            set => this.lastPlayType = value;
        }

        private bool hasLogMeStart = false;

        #endregion
    }
}