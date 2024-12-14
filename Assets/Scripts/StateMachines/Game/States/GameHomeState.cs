namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using BasePlayerInput.InputSystem;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using TheOneStudio.HyperCasual.Managers;
    using TheOneStudio.HyperCasual.Scenes.Main.HomeScreen;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.UITemplate.UITemplate.Models.Controllers;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Interface;

    public class GameHomeState : IGameState
    {
        private readonly IScreenManager                    screenManager;
        private readonly PlayerInputManager                playerInputManager;
        private readonly SongManager                       songManager;
        private readonly UITemplateInventoryDataController uiTemplateInventoryDataController;
        public GameHomeState(IScreenManager screenManager, PlayerInputManager playerInputManager, SongManager songManager, UITemplateInventoryDataController uiTemplateInventoryDataController)
        {
            this.screenManager                     = screenManager;
            this.playerInputManager                = playerInputManager;
            this.songManager                       = songManager;
            this.uiTemplateInventoryDataController = uiTemplateInventoryDataController;
        }
        public async void Enter()
        {
            this.TrySetDefaultDogCategory();
            this.songManager.SelectDefaultSong().Forget();
            this.playerInputManager.SetActive(false);
            this.screenManager.CloseAllScreen();
            this.screenManager.OpenScreen<HomeScreenPresenter>().Forget();
        }
        private void TrySetDefaultDogCategory()
        {
            if (string.IsNullOrEmpty(this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog1"))) this.uiTemplateInventoryDataController.UpdateCurrentSelectedItem("Dog1", this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog"));
            if (string.IsNullOrEmpty(this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog2"))) this.uiTemplateInventoryDataController.UpdateCurrentSelectedItem("Dog2", this.uiTemplateInventoryDataController.GetCurrentItemSelected("Dog"));
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}