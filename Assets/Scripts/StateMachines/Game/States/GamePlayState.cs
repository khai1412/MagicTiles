namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using BasePlayerInput.InputSystem;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using TheOneStudio.HyperCasual.Scenes.Main.GameplayScreen;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Interface;

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
            this.screenManager.OpenScreen<GameplayScreenPresenter>().Forget();
            //this.levelController.StartState();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}