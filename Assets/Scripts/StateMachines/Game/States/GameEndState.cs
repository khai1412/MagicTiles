namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Signals;
    using BasePlayerInput.InputSystem;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Signals;
    using TheOneStudio.HyperCasual.Managers;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Scenes.Main;
    using TheOneStudio.HyperCasual.Scenes.Popup;
    using TheOneStudio.HyperCasual.Utils;
    using TheOneStudio.UITemplate.UITemplate.Models;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Interface;


    public class GameEndState : IGameState
    {
        private readonly GlobalDataController  globalDataController;
        private readonly IScreenManager         screenManager;
        private readonly PlayerInputManager    playerInputManager;
        private readonly SongManager           songManager;
        private readonly AmaGDKAdsHelpers      amaGdkAdsHelpers;
        private readonly SignalBus             signalBus;
        private readonly AmaGDKAnalyticsHelper amaGdkAnalyticsHelper;

        private readonly UITemplateInventoryDataController uiTemplateInventoryDataController;

        public GameEndState(GlobalDataController              globalDataController, IScreenManager screenManager, PlayerInputManager playerInputManager, SongManager songManager, AmaGDKAdsHelpers amaGdkAdsHelpers, SignalBus signalBus, AmaGDKAnalyticsHelper amaGdkAnalyticsHelper,
                            UITemplateInventoryDataController uiTemplateInventoryDataController)
        {
            this.globalDataController  = globalDataController;
            this.screenManager         = screenManager;
            this.playerInputManager    = playerInputManager;
            this.songManager           = songManager;
            this.amaGdkAdsHelpers      = amaGdkAdsHelpers;
            this.signalBus             = signalBus;
            this.amaGdkAnalyticsHelper = amaGdkAnalyticsHelper;

            this.uiTemplateInventoryDataController = uiTemplateInventoryDataController;
        }

        public void Enter()
        {
            this.playerInputManager.SetActive(false);
            if (this.globalDataController.IsWin)
            {
                this.amaGdkAdsHelpers.ShowInter(StaticAdsPlacement.Inter.FinishGame, _ =>
                {
                    this.songManager.UnlockNextSong();
                    this.globalDataController.EndGame();
                    //If the level is tutorial level
                    if (this.globalDataController.IsGameplayTutorial)
                    {
                        this.screenManager.CloseAllScreen();
                        this.screenManager.OpenScreen<TutorialEndGameScreenPresenter>().Forget();
                    }

                    //Open if the level has the story outro
                    // else if (!string.IsNullOrEmpty(this.songManager.currentDuetDuetComposedLevelData.LevelRecord.StoryOutro) && !this.songManager.currentDuetDuetComposedLevelData.DogDuetLevelData.HasPassed)
                    // {
                    //     this.screenManager.OpenScreen<OutroScreenPresenter, OutroScreenModel>(new OutroScreenModel(this.songManager.currentDuetDuetComposedLevelData)).Forget();
                    // }

                    //If the level doesn't have outro, then show claim card
                    else if (this.uiTemplateInventoryDataController.GetItemData(this.songManager.currentDuetDuetComposedLevelData.LevelRecord.RewardCard).CurrentStatus != UITemplateItemData.Status.Owned)
                    {
                        if (this.globalDataController.IsCardTutorial)
                        {
                            this.globalDataController.IsCardTutorial = false;
                            this.screenManager.OpenScreen<PopupTutorialCardScreenPresenter>().Forget();
                        }
                        else this.screenManager.OpenScreen<PopupClaimCardScreenPresenter>().Forget();
                    }

                    //If the level doesn't have neither outro nor card, then skip
                    else
                    {
                        this.StateMachine.TransitionTo<GameOverState>();
                    }
                    this.songManager.PassCurrentSong();
                });
            }
            else
            {
                //If player is playing tutorial, revive instantly
                if (this.globalDataController.IsGameplayTutorial) this.Revive();
                else this.screenManager.OpenScreen<PopupRewardContinueScreenPresenter>().Forget();
            }
        }

        public void Exit() { }
        private void Revive()
        {
            this.LogReviveEvent(EventName.SongRevive);
            this.globalDataController.Revive();
            this.signalBus.Fire(new GameReviveSignal());
        }
        private void LogReviveEvent(string eventName)
        {
            AmaGDKLogModel eventModel = new AmaGDKLogModel
            {
                AcmId    = this.songManager.currentDuetDuetComposedLevelData.RemoteLevelRecord.acmSongID,
                SongName = this.songManager.currentDuetDuetComposedLevelData.RemoteLevelRecord.songName
            };
            this.amaGdkAnalyticsHelper.LogEvent(eventName, eventModel);
        }

        public IStateMachine StateMachine { get; set; }
    }
}