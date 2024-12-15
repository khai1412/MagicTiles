namespace MagicTiles.Scripts.Managers
{
    using MagicTiles.Scripts.Utils;
    using Services.Abstractions.AudioManager;

    public class PreviewSongManager
    {
        private readonly IAudioManager   audioManager;
        private readonly WebRequestUtils webRequestUtils;

        #region Inject

        public PreviewSongManager(IAudioManager audioManager, WebRequestUtils webRequestUtils)
        {
            this.audioManager    = audioManager;
            this.webRequestUtils = webRequestUtils;
        }

        #endregion

        private bool isPlayingPreview = false;
        public  bool IsPlayingPreview { get => this.isPlayingPreview; private set { this.isPlayingPreview = value; } }

        public async void PlayPreviewSong(string url)
        {
            this.isPlayingPreview = true;
            this.audioManager.PlayPlayList(await this.webRequestUtils.DownloadAudio(url), volumeScale: .5f);
        }

        public void ResumePreviewSong()
        {
            this.IsPlayingPreview = true;
            this.audioManager.ResumePlayList();
        }

        public void PausePreviewSong()
        {
            this.IsPlayingPreview = false;
            this.audioManager.PausePlayList();
        }

        public void TryContinuePreviewSong()
        {
            if (this.IsPlayingPreview) this.audioManager.ResumePlayList();
        }

        public void TempPausePreviewSong()
        {
            this.audioManager.PausePlayList();
        }

        public float GetPreviewPlaylistTime() => this.audioManager.GetPlayListTime();
    }
}