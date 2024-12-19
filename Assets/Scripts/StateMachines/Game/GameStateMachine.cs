namespace MagicTiles.Scripts.StateMachines.Game
{
    using System.Collections.Generic;
    using System.Linq;
    using BaseDuet.Scripts.Data.SessionData;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;
    using MagicTiles.Scripts.StateMachines.Game.States;
    using VContainer.Unity;
    using StateMachine = GameCore.Services.Implementations.StateMachine.StateMachine;

    public class GameStateMachine : StateMachine, IStartable
    {
        private readonly GlobalDataController globalDataController;

        public GameStateMachine(
            List<IGameState>     listState,
            GlobalDataController globalDataController
        )
            : base(listState.Select(state => state as IState).ToList())
        {
            this.globalDataController = globalDataController;
            listState.ForEach(x => x.StateMachine = this);
        }

        public void Start()
        {
            this.TransitionTo<GameHomeState>();
        }
    }
}