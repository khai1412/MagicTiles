namespace MagicTiles.Scripts.Managers
{
    using Cysharp.Threading.Tasks;
    using GameCore.Core.AssetsManager;
    using MagicTiles.Scripts.Models;
    using MagicTiles.Scripts.Utils;
    using UnityEngine;

    public class SongUtils
    {
        private readonly WebRequestUtils webRequestUtils;
        private readonly SongManager     songManager;
        private readonly IAssetManager   assetManager;

        public SongUtils(WebRequestUtils webRequestUtils, SongManager songManager, IAssetManager assetManager)
        {
            this.webRequestUtils = webRequestUtils;
            this.songManager     = songManager;
            this.assetManager    = assetManager;
        }

        public async UniTask<byte[]> GetSongMidiContent(DuetComposedLevelData levelData)
        {
            byte[] cacheMidiContent;
            cacheMidiContent = await this.webRequestUtils.DownloadMidiFile(levelData.RemoteLevelRecord.levelDesignUrl);
            return cacheMidiContent;
        }

        public TextAsset GetSongMidiContent(string path)
        {
            return this.assetManager.Load<TextAsset>(path);
        }

        public AudioClip GetSongAudio(string path)
        {
            return this.assetManager.Load<AudioClip>(path);
        }

        public async UniTask<AudioClip> GetSongAudio(DuetComposedLevelData levelData)
        {
            AudioClip cacheAudioClip;
            cacheAudioClip = await this.webRequestUtils.DownloadAudio(levelData.RemoteLevelRecord.mainAudioUrl);

            return cacheAudioClip;
        }
    }
}