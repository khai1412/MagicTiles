namespace BaseDuet.Scripts.Installers
{
    using BaseDuet.Scripts.Signals;
    using GameFoundation.Signals;
    using VContainer;

    public static class BaseDuetSignalInstaller 
    {
        public static void Configure(IContainerBuilder builder)
        {
            builder.DeclareSignal<GameOverSignal>();
            builder.DeclareSignal<NoteHitSignal>();
            builder.DeclareSignal<ObstacleTutorialSignal>();
            builder.DeclareSignal<ClaimTutRewardSignal>();
            builder.DeclareSignal<GameReviveSignal>();
            builder.DeclareSignal<DuetLevelChangeStateSignal>();
            builder.DeclareSignal<NoteDataChangeSignal>();
        }
    }
}