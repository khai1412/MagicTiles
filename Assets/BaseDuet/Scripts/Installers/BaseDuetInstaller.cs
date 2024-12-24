namespace BaseDuet.Scripts.Installers
{
    using BaseDuet.Scripts.Data.LocalData;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Helpers;
    using BaseDuet.Scripts.Levels;
    using BasePlayerInput.InputSystem;
    using GameCore.Extensions;
    using VContainer;
    using VContainer.Unity;

    public class BaseDuetInstaller
    {
        public static void Configure(IContainerBuilder builder)
        {
            builder.Register<GlobalDataController>(Lifetime.Singleton);
            builder.Register<BaseDuetCharacterViewHelper>(Lifetime.Singleton);
            builder.Register<GlobalData>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<LevelController>();
            builder.Register<BaseDuetLocalData>(Lifetime.Singleton);
            builder.Register<BaseDuetLocalDataController>(Lifetime.Singleton);
            builder.Register<PlayerInputManager>(Lifetime.Singleton).AsInterfacesAndSelf();
        }
    }
}