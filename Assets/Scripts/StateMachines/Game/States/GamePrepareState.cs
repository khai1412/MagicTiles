namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using Cysharp.Threading.Tasks;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;
    using Services.Abstractions.AudioManager;
    using Services.Abstractions.ScreenManager;
    using Services.Abstractions.StateMachine;

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