namespace Audio.Managers
{
    public interface IAudioManager
    {
        void Initialize();
        void PlaySfx(string sfxId);
        void PlayBackgroundMusic();
        void StopBackgroundMusic();
        void PauseBackgroundMusic();
    }
}