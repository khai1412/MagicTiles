namespace TheOneStudio.HyperCasual.StateMachines.Game.Interfaces
{
    using Services.Abstractions.StateMachine;

    public interface IGameState : IState
    {
        IStateMachine StateMachine { get; set; }

    }

    public interface IGameState<T> : IGameState
    {
        T Model { get; set; }
    }
}