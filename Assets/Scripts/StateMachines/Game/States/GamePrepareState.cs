using GameCore.Services.Abstractions.ScreenManager;

namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.BlueprintData;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Levels;
    using GameCore.Core.AudioManager;
    using GameCore.Core.StateMachine;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GamePrepareState : IGameState
    {
        private readonly IScreenManager       screenManager;
        private readonly IAudioManager        audioService;
        private readonly SongManager          songManager;
        private readonly MidiGenerator        midiGenerator;
        private readonly SongUtils            songUtils;
        private readonly LevelController      levelController;
        private readonly GlobalDataController globalDataController;

        public GamePrepareState(
            IScreenManager       screenManager,
            IAudioManager        audioService,
            SongManager          songManager,
            MidiGenerator        midiGenerator,
            SongUtils            songUtils,
            LevelController      levelController,
            GlobalDataController globalDataController
        )
        {
            this.screenManager        = screenManager;
            this.audioService         = audioService;
            this.songManager          = songManager;
            this.midiGenerator        = midiGenerator;
            this.songUtils            = songUtils;
            this.levelController      = levelController;
            this.globalDataController = globalDataController;
        }

        public async void Enter()
        {
            var levelModel  = this.songManager.GetAllLevelData()[this.globalDataController.CurrentSongId];
            var midiContent = this.songUtils.GetSongMidiContent(levelModel.LevelRecord.MidiAddress).bytes;
            var audioClip   = this.songUtils.GetSongAudio(levelModel.LevelRecord.SongAddress);
            await this.songManager.LoadSong(midiContent, audioClip, 0);
            this.levelController.PrepareState();
        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}