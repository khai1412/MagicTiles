namespace MagicTiles.Scripts.StateMachines.Game
{
    using System.Linq;
    using GameCore.Extensions;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;
    using VContainer;

    public class GameStateMachineInstaller
    {
        public static void Configure(IContainerBuilder builder)
        {
            builder.Register<GameStateMachine>(Lifetime.Singleton)
                .WithParameter(container => typeof(IGameState).GetDerivedTypes()
                    .Select(type => (IGameState)container.Instantiate(type))
                    .ToList())
                .AsInterfacesAndSelf();
        }
    }
}