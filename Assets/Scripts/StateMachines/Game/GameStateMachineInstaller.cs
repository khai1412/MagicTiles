namespace TheOneStudio.HyperCasual.StateMachines.Game
{
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.Utilities.Extension;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using VContainer;

    public class GameStateMachineInstaller
    {
        public static void Configure(IContainerBuilder builder)
        {
            builder.Register<GameStateMachine>(Lifetime.Singleton)
                   .WithParameter(container => typeof(IGameState).GetDerivedTypes().Select(type => (IGameState)container.Instantiate(type)).ToList())
                   .AsInterfacesAndSelf();
        }
    }
}