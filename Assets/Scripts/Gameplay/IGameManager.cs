namespace Gameplay
{
    public interface IGameManager
    {
        void Initialize();
        void Play();
        void Pause();
        void Resume();
        void Quit();
        void Stop();
    }
}