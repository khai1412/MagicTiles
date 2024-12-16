namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Levels;
    using GameCore.Core.AudioManager;
    using GameCore.Core.ScreenManager;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GamePrepareState : IGameState
    {
        private readonly IScreenManager  screenManager;
        private readonly IAudioManager   audioService;
        private readonly SongManager     songManager;
        private readonly MidiGenerator   midiGenerator;
        private readonly SongUtils       songUtils;
        private readonly LevelController levelController;

        public GamePrepareState(
            IScreenManager  screenManager,
            IAudioManager   audioService,
            SongManager     songManager,
            MidiGenerator   midiGenerator,
            SongUtils       songUtils,
            LevelController levelController
        )
        {
            this.screenManager   = screenManager;
            this.audioService    = audioService;
            this.songManager     = songManager;
            this.midiGenerator   = midiGenerator;
            this.songUtils       = songUtils;
            this.levelController = levelController;
        }

        public async void Enter()
        {
            // this.audioService.PlaySound(StaticSFXBlueprint.Instance.PlaySong);
            var midiContent = this.songUtils.GetSongMidiContent("Songs/Midi/GiaNhu").bytes;
            var audioClip   = this.songUtils.GetSongAudio("Songs/Audio/GiaNhu");
            await this.songManager.LoadSong(midiContent, audioClip, 0);
            this.levelController.PrepareState();
            // this.screenManager.OpenScreen<PreloadSongScreenPresenter>().Forget();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}