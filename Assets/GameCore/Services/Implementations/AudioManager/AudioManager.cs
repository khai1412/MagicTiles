using IAudioManager = GameCore.Core.AudioManager.IAudioManager;

namespace GameCore.Services.Implementations.AudioManager
{
    using UnityEngine;

    public class AudioManager : IAudioManager
    {
        public void  PlayPlayList(AudioClip audioClip, float volumeScale = 1)
        {
            throw new System.NotImplementedException();
        }

        public void  PausePlayList()
        {
            throw new System.NotImplementedException();
        }

        public void  ResumePlayList()
        {
            throw new System.NotImplementedException();
        }

        public float GetPlayListTime()
        {
            throw new System.NotImplementedException();
        }

        public void  PlaySound(AudioClip audioClip, float volumeScale = 1, bool isLoop = false)
        {
            throw new System.NotImplementedException();
        }

        public void  PlaySound(string    audioName, float volumeScale = 1, bool isLoop = false)
        {
            throw new System.NotImplementedException();
        }

        public void  PauseSound()
        {
            throw new System.NotImplementedException();
        }

        public void  ResumeSound()
        {
            throw new System.NotImplementedException();
        }

        public float GetSoundTime()
        {
            throw new System.NotImplementedException();
        }

        public void StopAllSound()
        {
            throw new System.NotImplementedException();
        }

        public void StopPlayList()
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayListPitch(float pitch)
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayListTime(float time)
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayListLoop(bool  isLoop)
        {
            throw new System.NotImplementedException();
        }
    }
}