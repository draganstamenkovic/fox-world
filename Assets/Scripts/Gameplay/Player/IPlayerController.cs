using UnityEngine;

namespace Gameplay.Player
{
    public interface IPlayerController
    {
        void Initialize(Transform gameplayParent);
        void Idle();
        void Move(float xValue);
        void Jump();
        void SetActive(bool active);
        Transform GetTransform();
    }
}