using IAudioManager = GameCore.Core.AudioManager.IAudioManager;

namespace GameCore.Services.Implementations.AudioManager
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameCore.Core.AssetsManager;
    using UnityEngine;

    public class AudioManager : MonoBehaviour, IAudioManager
    {
        private readonly IAssetManager assetManager;

        private readonly List<AudioSource>                        freeSoundSources = new();
        private readonly Dictionary<AudioClip, List<AudioSource>> usedSoundSources = new();
        private          AudioSource                              playListSources;

        public AudioManager(IAssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        private void InternalPlaySound(AudioClip audioClip, float volumeScale = 1, bool isLoop = false)
        {
            this.PlayFreeSoundSource(audioClip, volumeScale, isLoop);
        }

        private void PlayFreeSoundSource(AudioClip audioClip, float volumeScale, bool isLoop)
        {
            AudioSource source;
            if (this.freeSoundSources is { Count: > 0 })
            {
                source = this.freeSoundSources[0];
                this.freeSoundSources.RemoveAt(0);
            }
            else
            {
                source = Instantiate(new GameObject("SoundSource")).AddComponent<AudioSource>();
            }
            var sources = this.usedSoundSources.GetValueOrDefault(audioClip) ?? new();
            sources.Add(source);
            source.clip   = audioClip;
            source.volume = volumeScale;
            source.loop   = isLoop;
            source.Play();
            if (!isLoop)
            {
                UniTask.Delay((int)(audioClip.length * 1000)).ContinueWith(() =>
                {
                    this.freeSoundSources.Add(source);
                    this.usedSoundSources.Remove(audioClip);
                });
            }
        }

        private void PlayFreePlayListSource(AudioClip audioClip, float volumeScale, bool isLoop)
        {
            if (this.playListSources == null)
            {
                this.playListSources = Instantiate(new GameObject("PlayListSource")).AddComponent<AudioSource>();
            }
            this.playListSources.Stop();
            this.playListSources.clip   = audioClip;
            this.playListSources.volume = volumeScale;
            this.playListSources.loop   = isLoop;
            this.playListSources.Play();
        }

        void IAudioManager.PlayPlayList(AudioClip audioClip, float volumeScale, bool isLoop)
        {
            this.PlayFreePlayListSource(audioClip, volumeScale, isLoop);
        }

        void IAudioManager.PausePlayList()
        {
            this.playListSources.Pause();
        }

        void IAudioManager.ResumePlayList()
        {
            this.playListSources.UnPause();
        }

        float IAudioManager.GetPlayListTime()
        {
            return this.playListSources.time;
        }

        void IAudioManager.PlaySound(AudioClip audioClip, float volumeScale, bool isLoop)
        {
            if (audioClip == null)
            {
                Debug.LogError("AudioClip is null");
                return;
            }
            this.InternalPlaySound(audioClip, volumeScale, isLoop);
        }

        void IAudioManager.PlaySound(string audioName, float volumeScale, bool isLoop)
        {
            var audioClip = this.assetManager.Load<AudioClip>(audioName);
            if (audioClip == null)
            {
                Debug.LogError($"AudioClip {audioName} not found");
                return;
            }
            this.InternalPlaySound(audioClip, volumeScale, isLoop);
        }

        void IAudioManager.PauseSound(AudioClip audioClip)
        {
            if (this.usedSoundSources.TryGetValue(audioClip, out var source))
            {
                source.ForEach(s => s.Pause());
            }
        }

        void IAudioManager.PauseSound(string audioName)
        {
            var audioClip = this.assetManager.Load<AudioClip>(audioName);
            if (audioClip == null)
            {
                Debug.LogError($"AudioClip {audioName} not found");
                return;
            }
            if (this.usedSoundSources.TryGetValue(audioClip, out var source))
            {
                source.ForEach(s => s.Pause());
            }
        }

        void IAudioManager.ResumeSound(AudioClip audioClip)
        {
            if (this.usedSoundSources.TryGetValue(audioClip, out var source))
            {
                source.ForEach(s => s.UnPause());
            }
        }

        void IAudioManager.ResumeSound(string audioName)
        {
            var audioClip = this.assetManager.Load<AudioClip>(audioName);
            if (audioClip == null)
            {
                Debug.LogError($"AudioClip {audioName} not found");
                return;
            }
            if (this.usedSoundSources.TryGetValue(audioClip, out var source))
            {
                source.ForEach(s => s.UnPause());
            }
        }

        void IAudioManager.StopAllSound()
        {
            foreach (var usedSoundSource in this.usedSoundSources.Values)
            {
                usedSoundSource.ForEach(s => s.Stop());
            }
        }

        void IAudioManager.StopPlayList()
        {
            this.playListSources.Stop();
        }

        void IAudioManager.SetPlayListPitch(float pitch)
        {
            this.playListSources.pitch = pitch;
        }

        void IAudioManager.SetPlayListTime(float time)
        {
            this.playListSources.time = time;
        }

        public void SetPlayListLoop(bool isLoop)
        {
            this.playListSources.loop = isLoop;
        }
    }
}