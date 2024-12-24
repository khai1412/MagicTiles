namespace GameCore.Scripts
{
    using BaseDuet.Scripts.Installers;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.StateMachines.Game;
    using VContainer;
    using VContainer.Unity;

    public class MainSceneScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<SongManager>(Lifetime.Singleton);
            builder.Register<PreviewSongManager>(Lifetime.Singleton);
            builder.Register<MidiGenerator>(Lifetime.Singleton);
            builder.Register<SongUtils>(Lifetime.Singleton);
            BaseDuetInstaller.Configure(builder);
            GameStateMachineInstaller.Configure(builder);
            
        }
    }
}