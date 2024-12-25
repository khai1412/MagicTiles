namespace MagicTiles.Scripts.StateMachines.Game.States
{
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Levels;
    using BasePlayerInput.InputSystem;
    using GameCore.Core.AudioManager;
    using GameCore.Core.StateMachine;
    using GameCore.Services.Abstractions.ScreenManager;
    using MagicTiles.Scripts.Managers;
    using MagicTiles.Scripts.MIDI;
    using MagicTiles.Scripts.StateMachines.Game.Interfaces;

    public class GamePrepareState : IGameState
    {
        private readonly IAudioManager        audioService;
        private readonly GlobalDataController globalDataController;
        private readonly PlayerInputManager   playerInputManager;
        private readonly LevelController      levelController;
        private readonly MidiGenerator        midiGenerator;
        private readonly IScreenManager       screenManager;
        private readonly SongManager          songManager;
        private readonly SongUtils            songUtils;

        public GamePrepareState(
            IScreenManager       screenManager,
            IAudioManager        audioService,
            SongManager          songManager,
            MidiGenerator        midiGenerator,
            SongUtils            songUtils,
            LevelController      levelController,
            GlobalDataController globalDataController,
            PlayerInputManager playerInputManager
        )
        {
            this.screenManager        = screenManager;
            this.audioService         = audioService;
            this.songManager          = songManager;
            this.midiGenerator        = midiGenerator;
            this.songUtils            = songUtils;
            this.levelController      = levelController;
            this.globalDataController = globalDataController;
            this.playerInputManager   = playerInputManager;
        }

        public async void Enter()
        {
            var levelModel  = this.songManager.GetAllLevelData()[this.globalDataController.CurrentSongId];
            var a           = this.songUtils.GetSongMidiContent(levelModel.LevelRecord.MidiAddress);
            var midiContent = this.songUtils.GetSongMidiContent(levelModel.LevelRecord.MidiAddress).bytes;
            var audioClip   = this.songUtils.GetSongAudio(levelModel.LevelRecord.SongAddress);
            await this.songManager.LoadSong(midiContent, audioClip);
            this.levelController.PrepareState();
            this.globalDataController.PlayGame();
            this.levelController.StartState();
            this.playerInputManager.ResetModuleList();

        }

        public void          Exit()       { }
        public IStateMachine StateMachine { get; set; }
    }
}