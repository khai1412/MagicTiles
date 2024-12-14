namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities;
    using TheOneStudio.HyperCasual.Scenes.Main;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Interface;

    public class GamePrepareState : IGameState
    {
        private readonly IScreenManager screenManager;
        private readonly IAudioService  audioService;

        public GamePrepareState(IScreenManager screenManager, IAudioService audioService)
        {
            this.screenManager = screenManager;
            this.audioService  = audioService;
        }

        public void Enter()
        {
            this.audioService.PlaySound(StaticSFXBlueprint.Instance.PlaySong);
            this.screenManager.OpenScreen<PreloadSongScreenPresenter>().Forget();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}