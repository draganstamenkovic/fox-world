namespace Gameplay.Level
{
    public interface ILevelManager
    {
        void Initialize();
        void LoadLevel(string id);
        void LoadNextLevel();
        
    }
}