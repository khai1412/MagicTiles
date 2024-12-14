namespace TheOneStudio.HyperCasual.StateMachines.Game
{
    using System.Collections.Generic;
    using System.Linq;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Signals;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Signals;
    using TheOneStudio.HyperCasual.Models;
    using TheOneStudio.HyperCasual.Others.StateMachine.Interface;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.HyperCasual.StateMachines.Game.States;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Controller;
    using VContainer.Unity;

    public class GameStateMachine : StateMachine, IStartable
    {
        private readonly UserLocalDataController userLocalDataController;
        private readonly GlobalDataController    globalDataController;

        public GameStateMachine(List<IGameState>        listState,
                                ILogService             logService,
                                UserLocalDataController userLocalDataController,
                                GlobalDataController    globalDataController,
                                SignalBus               signalBus)
            : base(listState.Select(state => state as IState).ToList(), logService, signalBus)
        {
            this.userLocalDataController = userLocalDataController;
            this.globalDataController    = globalDataController;
            listState.ForEach(x => x.StateMachine = this);
        }

        public void Start()
        {
            if (this.userLocalDataController.IsFirstOpen || this.globalDataController.IsGameplayTutorial)
            {
                this.TransitionTo<GameTutorialState>();
            }
            else
            {
                this.TransitionTo<GameHomeState>();
            }

            this.signalBus.Subscribe<GameOverSignal>(this.TransitionTo<GameEndState>);
        }
    }
}