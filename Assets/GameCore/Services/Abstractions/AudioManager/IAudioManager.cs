namespace Services.Abstractions.AudioManager
{
    using UnityEngine;

    public interface IAudioManager
    {
        void  PlayPlayList(AudioClip audioClip, float volumeScale = 1f);
        void  PausePlayList();
        void  ResumePlayList();
        float GetPlayListTime();

        void  PlaySound(AudioClip audioClip, float volumeScale = 1f);
        void  PauseSound();
        void  ResumeSound();
        float GetSoundTime();
    }
}