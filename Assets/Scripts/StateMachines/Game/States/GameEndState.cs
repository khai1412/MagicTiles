namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Levels;
    using BasePlayerInput.InputSystem;
    using GameCore.Core.StateMachine;
    using GameCore.Services.Abstractions.ScreenManager;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GameEndState : IGameState
    {
        private readonly GlobalDataController globalDataController;
        private readonly PlayerInputManager   playerInputManager;
        private readonly IScreenManager       screenManager;
        private readonly SongManager          songManager;
        private readonly LevelController      levelController;

        public GameEndState(
            GlobalDataController globalDataController,
            IScreenManager screenManager,
            PlayerInputManager playerInputManager,
            SongManager songManager,
            LevelController levelController
        )
        {
            this.globalDataController = globalDataController;
            this.screenManager        = screenManager;
            this.playerInputManager   = playerInputManager;
            this.songManager          = songManager;
            this.levelController      = levelController;
        }

        public void Enter()
        {
            this.playerInputManager.SetActive(false);
            if (this.globalDataController.IsWin)
            {
                this.songManager.UnlockNextSong();
                this.globalDataController.EndGame();
                this.levelController.EndState();
                this.StateMachine.TransitionTo<GameOverState>();
                this.songManager.PassCurrentSong();
            }
            else
            {
                //If player is playing tutorial, revive instantly
                if (this.globalDataController.IsGameplayTutorial) this.Revive();
                // this.screenManager.OpenScreen<PopupRewardContinueScreenPresenter>().Forget();
            }
        }

        public void Exit() { }

        public IStateMachine StateMachine { get; set; }

        private void Revive() { this.globalDataController.Revive(); }
    }
}