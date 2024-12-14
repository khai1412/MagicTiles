namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.SessionData;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using TheOneStudio.HyperCasual.Scenes.Popup;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Interface;

    public class GameOverState : IGameState
    {
        private readonly IScreenManager        screenManager;
        private readonly GlobalDataController globalDataController;

        #region Inject

        public GameOverState(IScreenManager screenManager, GlobalDataController globalDataController)
        {
            this.screenManager        = screenManager;
            this.globalDataController = globalDataController;
        }

        #endregion

        public async void Enter()
        {
            this.screenManager.CloseAllScreen();
            await this.screenManager.OpenScreen<GameOverScreenPresenter>();
            
        }
        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}