namespace GameCore.Scripts
{
    using GameCore.Services.Implementations.ScreenManager;
    using VContainer;
    using VContainer.Unity;

    public class LoadingSceneScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
        }
    }
}