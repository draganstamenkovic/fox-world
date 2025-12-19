using Gameplay.Player;
using Helpers.RuntimeInfo;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [Inject] private readonly IRuntimeInformation _runtimeInformation;
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private UIButtonInput jumpButton;
        
        private IPlayerController _playerController;
        
        private float _moveValue;
        private bool _isActive;

        private InputAction _moveAction;
        private InputAction _jumpAction;

        private bool _jumpPressed;
        private bool _jumpConsumed;
        public void Initialize(IPlayerController playerController)
        {
            _playerController = playerController;

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            jumpButton.OnPressed += UIJumpStarted;
            jumpButton.OnReleased += UIJumpCanceled;
        }

        public void SetActive(bool value)
        {
            if (value)
            {
#if UNITY_Android || UNITY_IOS
                jumpButton.gameObject.SetActive(true);
                joystick.gameObject.SetActive(true);
#endif
                _jumpAction.started += OnJumpStarted;
                _jumpAction.canceled += OnJumpCanceled;
            }
            else
            {
#if UNITY_Android || UNITY_IOS
                joystick.SetActive(false);
#endif
                _jumpAction.started -= OnJumpStarted;
                _jumpAction.canceled -= OnJumpCanceled;
            }
            _isActive = value;
        }
        private void UIJumpStarted()
        {
            _jumpPressed = true;
            _jumpConsumed = false;
        }

        private void UIJumpCanceled()
        {
            _jumpPressed = false;
        }

        private void OnJumpCanceled(InputAction.CallbackContext obj)
        {
            _jumpPressed = false;
        }

        private void OnJumpStarted(InputAction.CallbackContext obj)
        {
            _jumpPressed = true;
            _jumpConsumed = false;
        }

        private void Update()
        {
            if (!_isActive) return;
            
#if UNITY_ANDROID || UNITY_IOS
            _moveValue = joystick.Direction.x;
#endif
            if(_moveAction.IsPressed())
                _moveValue = _moveAction.ReadValue<Vector2>().x;
            if(!_moveAction.IsPressed())
                _playerController.Idle();
        }

        private void FixedUpdate()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (!_isActive) return;
            
            if (_moveValue == 0)
                _playerController.Idle();
            else
                _playerController.Move(_moveValue);
#endif      
            
            if(_moveAction.IsPressed())
                _playerController.Move(_moveValue);

            if (_jumpPressed && !_jumpConsumed)
            {
                _playerController.Jump();
                _jumpConsumed = true;
            }
        }
    }
}