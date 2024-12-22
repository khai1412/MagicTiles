namespace MagicTiles.Scripts.Managers
{
    using GameCore.Core.AudioManager;
    using MagicTiles.Scripts.Utils;

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

        public bool IsPlayingPreview { get; private set; } = false;

        public async void PlayPreviewSong(string url)
        {
            this.IsPlayingPreview = true;
            //this.audioManager.PlayPlayList(await this.webRequestUtils.DownloadAudio(url), .5f);
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

        public float GetPreviewPlaylistTime()
        {
            return this.audioManager.GetPlayListTime();
        }
    }
}