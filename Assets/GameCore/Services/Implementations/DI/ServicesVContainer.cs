namespace GameCore.Services.Implementations.DI
{
    using GameCore.Services.Abstractions.LocalData;
    using GameCore.Services.Implementations.AssetsManager;
    using GameCore.Services.Implementations.AudioManager;
    using GameCore.Services.Implementations.LocalData;
    using GameCore.Services.Implementations.ObjectPool;
    using GameCore.Services.Implementations.ScreenManager;
    using VContainer;
    using GameCore.Extensions;

    public static class ServicesVContainer
    {
        public static void RegisterGameCore(this IContainerBuilder builder)
        {
            builder.Register<ObjectPoolManager>(Lifetime.Singleton);
            builder.Register<LocalDataHandler>(Lifetime.Singleton);
            builder.Register<AssetsManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AudioManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScreenManager>(Lifetime.Singleton).AsInterfacesAndSelf();

            // foreach (var type in typeof(ILocalData).GetDerivedTypes())
            // {
            //     builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces();
            // }
            //
            // foreach (var type in typeof(ILocalDataController).GetDerivedTypes())
            // {
            //     builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces();
            // }
            // builder.Register<LocalDataHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}