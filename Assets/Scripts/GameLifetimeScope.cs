namespace GameCore.Scripts
{
    using VContainer;
    using VContainer.Unity;
    using GameCore.Services.Implementations.DI;

    public class ThisGameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterServices();
        }
    }
}