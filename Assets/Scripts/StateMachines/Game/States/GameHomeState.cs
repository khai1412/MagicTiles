namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BasePlayerInput.InputSystem;
    using Cysharp.Threading.Tasks;
    using GameCore.Core.StateMachine;
    using GameCore.Services.Abstractions.ScreenManager;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GameHomeState : IGameState
    {
        private readonly PlayerInputManager playerInputManager;
        private readonly IScreenManager     screenManager;
        private readonly SongManager        songManager;

        public GameHomeState(
            IScreenManager     screenManager,
            PlayerInputManager playerInputManager,
            SongManager        songManager
        )
        {
            this.screenManager      = screenManager;
            this.playerInputManager = playerInputManager;
            this.songManager        = songManager;
        }

        public async void Enter()
        {
            this.songManager.SelectDefaultSong().Forget();
            this.playerInputManager.SetActive(false);
            this.screenManager.CloseAllScreen();
            // this.screenManager.OpenScreen<HomeScreenPresenter>().Forget();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}