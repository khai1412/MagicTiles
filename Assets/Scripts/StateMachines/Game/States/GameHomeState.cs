namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BasePlayerInput.InputSystem;
    using Cysharp.Threading.Tasks;
    using GameCore.Core.ScreenManager;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GameHomeState : IGameState
    {
        private readonly IScreenManager     screenManager;
        private readonly PlayerInputManager playerInputManager;
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
            this.TrySetDefaultDogCategory();
            this.songManager.SelectDefaultSong().Forget();
            this.playerInputManager.SetActive(false);
            this.screenManager.CloseAllScreen();
            // this.screenManager.OpenScreen<HomeScreenPresenter>().Forget();
        }

        private void TrySetDefaultDogCategory()
        {
            // if (string.IsNullOrEmpty(this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog1"))) this.uiTemplateInventoryDataController.UpdateCurrentSelectedItem("Dog1", this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog"));
            // if (string.IsNullOrEmpty(this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog2"))) this.uiTemplateInventoryDataController.UpdateCurrentSelectedItem("Dog2", this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog"));
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}