namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.SessionData;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;
    using Services.Abstractions.ScreenManager;
    using Services.Abstractions.StateMachine;

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
            // await this.screenManager.OpenScreen<GameOverScreenPresenter>();

        }
        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}