namespace MagicTiles.Scripts.StateMachines.Game.Interfaces
{
    using GameCore.Core.StateMachine;

    public interface IGameState : IState
    {
        IStateMachine StateMachine { get; set; }

    }
}