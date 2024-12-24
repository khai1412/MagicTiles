namespace GameCore.Scripts
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Installers;
    using GameCore.Services.Implementations.DI;
    using GameCore.Services.Implementations.ScreenManager;
    using MagicTiles.Scripts.Blueprints;
    using MagicTiles.Scripts.Helpers;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.Models;
    using MagicTiles.Scripts.StateMachines.Game;
    using MagicTiles.Scripts.Utils;
    using UnityEngine;
    using VContainer;
    using VContainer.Unity;

    public class ThisGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private StaticValueBlueprint staticValueBlueprint;
        [SerializeField] private StaticSFXBlueprint   staticSFXBlueprint;
        [SerializeField] private LevelBlueprint       levelBlueprint;
        [SerializeField] private RemoteLevelBlueprint remoteLevelBlueprint;
        [SerializeField] private UIConfigBlueprint    uiConfigBlueprint;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterGameCore();
            

            builder.Register<UserLocalData>(Lifetime.Singleton);
            builder.Register<UserLocalDataController>(Lifetime.Singleton);

            builder.RegisterInstance(this.staticSFXBlueprint);
            builder.RegisterInstance(this.staticValueBlueprint);
            builder.RegisterInstance(this.levelBlueprint);
            builder.RegisterInstance(this.remoteLevelBlueprint);
            builder.RegisterInstance(this.uiConfigBlueprint);


            builder.Register<TimeHelper>(Lifetime.Singleton);

            builder.Register<WebRequestUtils>(Lifetime.Singleton);

        }
    }
}