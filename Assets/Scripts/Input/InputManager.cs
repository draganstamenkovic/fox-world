using Gameplay.Player;
using Message;
using Message.Messages;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private UIButtonInput jumpButton;
        [Inject] private IMessageBroker _messageBroker;
        
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
            _messageBroker.Receive<DisableInputMessage>().Subscribe(message => SetActive(false));
        }

        public void SetActive(bool value)
        {
            if (value)
            {
#if ANDROID || IOS
                jumpButton.gameObject.SetActive(true);
                joystick.gameObject.SetActive(true);
                joystick.handle.anchoredPosition = Vector2.zero;
                joystick.input = Vector2.zero;
#endif
                _jumpAction.started += OnJumpStarted;
                _jumpAction.canceled += OnJumpCanceled;
            }
            else
            {
#if ANDROID || IOS
                joystick.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
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
            
#if ANDROID || IOS
            _moveValue = joystick.Direction.x;
#elif UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL   
            if(_moveAction.IsPressed())
                _moveValue = _moveAction.ReadValue<Vector2>().x;
            else
                _playerController.Idle();
#endif
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
            
#if ANDROID || IOS   
            if (_moveValue == 0)
                _playerController.Idle();
            else
                _playerController.Move(_moveValue);
            
#elif UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if(_moveAction.IsPressed())
                _playerController.Move(_moveValue);
#endif
            if (_jumpPressed && !_jumpConsumed)
            {
                _playerController.Jump();
                _jumpConsumed = true;
            }
        }
    }
}