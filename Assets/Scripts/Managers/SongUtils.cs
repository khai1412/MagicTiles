namespace MagicTiles.Scripts.Managers
{
    using Cysharp.Threading.Tasks;
    using MagicTiles.Scripts.Models;
    using MagicTiles.Scripts.Utils;
    using UnityEngine;

    public class SongUtils
    {
        private readonly WebRequestUtils webRequestUtils;
        private readonly SongManager     songManager;
        public SongUtils(WebRequestUtils webRequestUtils, SongManager songManager)
        {
            this.webRequestUtils = webRequestUtils;
            this.songManager     = songManager;
        }
        public async UniTask<byte[]> GetSongMidiContent(DuetComposedLevelData levelData)
        {
            byte[] cacheMidiContent;
            cacheMidiContent = await this.webRequestUtils.DownloadMidiFile(levelData.RemoteLevelRecord.levelDesignUrl);
            return cacheMidiContent;
        }
        public async UniTask<AudioClip> GetSongAudio(DuetComposedLevelData levelData)
        {
            AudioClip cacheAudioClip;
            cacheAudioClip = await this.webRequestUtils.DownloadAudio(levelData.RemoteLevelRecord.mainAudioUrl);

            return cacheAudioClip;
        }
    }
}