using Configs;
using Gameplay.Player;
using Helpers.RuntimeInfo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [Inject] private readonly IRuntimeInformation _runtimeInformation;
        [SerializeField] private InputConfig _inputConfig;
        private IPlayerController _playerController;
        private bool _isActive;
        
        public void Initialize(IPlayerController playerController)
        {
            _playerController = playerController;
            if(_runtimeInformation.OSPlatform == RuntimeOSPlatform.Editor)
                InitializePCInput();
            else
                InitializeMobileInput();
        }

        private void InitializeMobileInput()
        {
            // TODO: Add missing logic here
        }

        private void InitializePCInput()
        {
            // TODO: Add missing logic here
        }

        public void SetActive(bool value)
        {
            if (_runtimeInformation.OSPlatform == RuntimeOSPlatform.Editor)
            {
                // TODO: Add logic for Editor
            }
            else
            {
                // TODO: Add input logic for mobile platforms
            }

            _isActive = value;
        }

        private void Update()
        {
            if (!_isActive) return;
            if (_runtimeInformation.OSPlatform == RuntimeOSPlatform.Editor)
            {
                var moveValue = new Vector2();
                _playerController.Move(moveValue);
            }
            else
            {
                // TODO: Add touch input
            }
        }
    }
}