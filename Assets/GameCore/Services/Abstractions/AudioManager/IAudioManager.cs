namespace GameCore.Core.AudioManager
{
    using UnityEngine;

    public interface IAudioManager
    {
        void  PlayPlayList(AudioClip audioClip, float volumeScale = 1f);
        void  PausePlayList();
        void  ResumePlayList();
        float GetPlayListTime();

        void  PlaySound(AudioClip audioClip, float volumeScale = 1f, bool isLoop = false);
        void  PlaySound(string    audioName, float volumeScale = 1f, bool isLoop = false);
        void  PauseSound();
        void  ResumeSound();
        float GetSoundTime();

        void StopAllSound();
        void StopPlayList();
        void SetPlayListPitch(float pitch);

        void SetPlayListTime(float time);
        void SetPlayListLoop(bool  isLoop);
    }
}