namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.SessionData;
    using GameCore.Core.StateMachine;
    using GameCore.Services.Abstractions.ScreenManager;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GameOverState : IGameState
    {
        private readonly GlobalDataController globalDataController;
        private readonly IScreenManager       screenManager;

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
            //await this.screenManager.OpenScreen<GameOverScreenPresenter>();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}