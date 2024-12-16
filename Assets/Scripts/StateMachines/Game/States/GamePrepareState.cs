namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using GameCore.Core.AudioManager;
    using GameCore.Core.ScreenManager;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GamePrepareState : IGameState
    {
        private readonly IScreenManager screenManager;
        private readonly IAudioManager  audioService;

        public GamePrepareState(IScreenManager screenManager, IAudioManager audioService)
        {
            this.screenManager = screenManager;
            this.audioService  = audioService;
        }

        public void Enter()
        {
            this.audioService.PlaySound(StaticSFXBlueprint.Instance.PlaySong);
            // this.screenManager.OpenScreen<PreloadSongScreenPresenter>().Forget();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}