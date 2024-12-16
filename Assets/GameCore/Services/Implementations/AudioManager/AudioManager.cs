using IAudioManager = GameCore.Core.AudioManager.IAudioManager;

namespace GameCore.Services.Implementations.AudioManager
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : IAudioManager
    {
        private List<AudioSource> freeSoundSources = new();
        private List<AudioSource> usedSoundSources = new();

        private List<AudioSource> freePlayListSources = new();
        private List<AudioSource> usedPlayListSources = new();

        private void InternalPlaySound(AudioClip audioClip, float volumeScale = 1, bool isLoop = false)
        {
        }


        public void PlayPlayList(AudioClip audioClip, float volumeScale = 1)
        {
        }

        public void PausePlayList()
        {
        }

        public void ResumePlayList()
        {
        }

        public float GetPlayListTime()
        {
            return 0;
        }

        public void PlaySound(AudioClip audioClip, float volumeScale = 1, bool isLoop = false)
        {
        }

        public void PlaySound(string audioName, float volumeScale = 1, bool isLoop = false)
        {
        }

        public void PauseSound()
        {
        }

        public void ResumeSound()
        {
        }

        public float GetSoundTime()
        {
            return 0;
        }

        public void StopAllSound()
        {
        }

        public void StopPlayList()
        {
        }

        public void SetPlayListPitch(float pitch)
        {
        }

        public void SetPlayListTime(float time)
        {

        }

        public void SetPlayListLoop(bool isLoop)
        {

        }
    }
}