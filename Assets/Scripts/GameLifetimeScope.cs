namespace GameCore.Scripts
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using VContainer;
    using VContainer.Unity;
    using GameCore.Services.Implementations.DI;
    using BaseDuet.Scripts.Installers;
    using MagicTiles.Scripts.Blueprints;
    using MagicTiles.Scripts.Helpers;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.Models;
    using MagicTiles.Scripts.StateMachines.Game;
    using MagicTiles.Scripts.Utils;
    using UnityEngine;

    public class ThisGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private StaticValueBlueprint staticValueBlueprint;
        [SerializeField] private StaticSFXBlueprint   staticSFXBlueprint;
        [SerializeField] private LevelBlueprint       levelBlueprint;
        [SerializeField] private RemoteLevelBlueprint remoteLevelBlueprint;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterGameCore();
            BaseDuetInstaller.Configure(builder);
            GameStateMachineInstaller.Configure(builder);

            builder.Register<UserLocalData>(Lifetime.Singleton);
            builder.Register<UserLocalDataController>(Lifetime.Singleton);

            builder.RegisterInstance(this.staticSFXBlueprint);
            builder.RegisterInstance(this.staticValueBlueprint);
            builder.RegisterInstance(this.levelBlueprint);
            builder.RegisterInstance(this.remoteLevelBlueprint);

            builder.Register<SongManager>(Lifetime.Singleton);
            builder.Register<PreviewSongManager>(Lifetime.Singleton);
            builder.Register<MidiGenerator>(Lifetime.Singleton);

            builder.Register<TimeHelper>(Lifetime.Singleton);

            builder.Register<WebRequestUtils>(Lifetime.Singleton);
            builder.Register<SongUtils>(Lifetime.Singleton);
        }
    }
}