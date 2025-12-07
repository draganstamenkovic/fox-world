namespace Gameplay
{
    public interface IScoreManager
    {
        void IncreaseCherryCount();
        int GetCherryCount();
        void IncreaseGemCount();
        int GetGemCount();
    }
}