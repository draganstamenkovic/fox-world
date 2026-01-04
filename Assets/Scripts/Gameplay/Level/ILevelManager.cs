using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Level
{
    public interface ILevelManager
    {
        void Initialize();
        Task LoadLevel();
        void LoadNextLevel();
        void DestroyActiveLevel();
        BoxCollider2D GetLevelCollider();
    }
}