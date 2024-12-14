namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using System.Linq;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Levels;
    using BaseDuet.Scripts.Models;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using TheOneStudio.HyperCasual.Blueprints;
    using TheOneStudio.HyperCasual.Managers;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Models.StaticModel;
    using TheOneStudio.HyperCasual.Scenes.Main.StorySplashScreen;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.HyperCasual.Utils;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Interface;
    using UnityEngine;

    public class GameTutorialState : IGameState
    {
        private readonly GlobalDataController    globalDataController;
        private readonly SongManager             songManager;
        private readonly UserLocalDataController userLocalDataController;
        private readonly IScreenManager           screenManager;
        private readonly LevelController         levelController;
        private readonly AmaGDKAnalyticsHelper   amaGdkAnalyticsHelper;
        private readonly LevelBlueprint          levelBlueprint;
        private readonly IGameAssets             gameAssets;

        #region Inject

        public GameTutorialState(GlobalDataController globalDataController, SongManager songManager, UserLocalDataController userLocalDataController, IScreenManager screenManager, LevelController levelController, AmaGDKAnalyticsHelper amaGdkAnalyticsHelper, LevelBlueprint levelBlueprint, IGameAssets gameAssets)
        {
            this.globalDataController    = globalDataController;
            this.songManager             = songManager;
            this.userLocalDataController = userLocalDataController;
            this.screenManager           = screenManager;
            this.levelController         = levelController;
            this.amaGdkAnalyticsHelper   = amaGdkAnalyticsHelper;
            this.levelBlueprint          = levelBlueprint;
            this.gameAssets              = gameAssets;
        }

        #endregion

        public async void Enter()
        {
            
            this.globalDataController.IsGameplayTutorial = true;
            this.globalDataController.IsShopTutorial     = true;
            this.globalDataController.IsSongTutorial     = true;
            this.globalDataController.IsObstacleTutorial = true;
            this.globalDataController.IsCardTutorial     = true;
            this.globalDataController.IsAlbumTutorial    = true;

            var midiContent = this.gameAssets.LoadAssetAsync<TextAsset>(StaticTutorialSong.Midi).WaitForCompletion();
            var audioClip   = this.gameAssets.LoadAssetAsync<AudioClip>(StaticTutorialSong.Song).WaitForCompletion();
            this.songManager.SelectTutorialSong();
            this.LogEventAp();
            await this.songManager.LoadSong(midiContent.bytes, audioClip, 0);
            this.levelController.PrepareState();
            this.userLocalDataController.IsFirstOpen = false;
            var firstLevelStory = this.songManager.GetAllLevelData().OrderBy(x => x.Value.RemoteLevelRecord.Story).First().Value;
            this.globalDataController.SetNoteSpeed("Easy",ESongSegment.Intro);
            await this.screenManager.OpenScreen<StorySplashScreenPresenter, StorySplashScreenModel>(new()
            {
                DuetLevelComposed = firstLevelStory
            });
        }
        private void LogEventAp()
        {
            this.amaGdkAnalyticsHelper.LogEvent(EventName.SongAp, new AmaGDKLogModel()
            {
                AcmId        = this.songManager.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                SongName     = this.songManager.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName,
                SongPlayType = this.songManager.LastPlayType.ToString()
            });
        }
        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}