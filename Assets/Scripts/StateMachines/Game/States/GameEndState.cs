using GameCore.Services.Abstractions.ScreenManager;

namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.SessionData;
    using BasePlayerInput.InputSystem;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GameEndState : IGameState
    {
        private readonly GlobalDataController globalDataController;
        private readonly IScreenManager       screenManager;
        private readonly PlayerInputManager   playerInputManager;
        private readonly SongManager          songManager;

        public GameEndState(
            GlobalDataController globalDataController,
            IScreenManager       screenManager,
            PlayerInputManager   playerInputManager,
            SongManager          songManager
        )
        {
            this.globalDataController = globalDataController;
            this.screenManager        = screenManager;
            this.playerInputManager   = playerInputManager;
            this.songManager          = songManager;
        }

        public void Enter()
        {
            this.playerInputManager.SetActive(false);
            if (this.globalDataController.IsWin)
            {
                this.songManager.UnlockNextSong();
                this.globalDataController.EndGame();
                this.StateMachine.TransitionTo<GameOverState>();
                this.songManager.PassCurrentSong();
            }
            else
            {
                //If player is playing tutorial, revive instantly
                if (this.globalDataController.IsGameplayTutorial)
                    this.Revive();
                else
                {
                    // this.screenManager.OpenScreen<PopupRewardContinueScreenPresenter>().Forget();
                }
            }
        }

        public void Exit() { }

        private void Revive()
        {
            this.globalDataController.Revive();
        }

        public IStateMachine StateMachine { get; set; }
    }
}