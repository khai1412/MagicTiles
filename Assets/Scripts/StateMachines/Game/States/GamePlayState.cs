namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BasePlayerInput.InputSystem;
    using GameCore.Core.ScreenManager;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GamePlayState : IGameState
    {
        private readonly PlayerInputManager playerInputManager;
        private readonly IScreenManager      screenManager;
        public GamePlayState(PlayerInputManager playerInputManager, IScreenManager screenManager)
        {
            this.playerInputManager = playerInputManager;
            this.screenManager      = screenManager;
        }
        public void Enter()
        {
            // this.screenManager.OpenScreen<GameplayScreenPresenter>().Forget();
            //this.levelController.StartState();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}