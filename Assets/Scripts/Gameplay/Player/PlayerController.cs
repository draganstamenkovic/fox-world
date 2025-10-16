using Audio.Managers;
using Cameras;
using Configs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Player
{
    public class PlayerController : IPlayerController
    {
        [Inject] private PlayerConfig _playerConfig;
        [Inject] private ICameraManager _cameraManager;
        [Inject] private IAudioManager _audioManager;
        private readonly IObjectResolver _objectResolver;

        private PlayerView _playerView;
        
        private Vector3 _startPosition;
        
        private bool _isGrounded;
        private bool _isJumping;
        private bool _isIdle;
        
       // private float _lastGroundedTime;
        
        public PlayerController(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        public void Initialize(Transform gameplayParent)
        {
            var player = _objectResolver.Instantiate(_playerConfig.playerPrefab, gameplayParent);
            player.SetActive(false);
            _playerView = player.GetComponent<PlayerView>();
            _playerView.OnCollision = OnCollisionEnter;

            _playerView.rigidBody.gravityScale = _playerConfig.defaultGravity;
        }
        private void OnCollisionEnter(Collision2D collision)
        {
            /*
            if (collision.gameObject.tag.Equals("Ground"))
            {
                _isGrounded = true;
                _isJumping = false;
            }
            */
        }
        public void Idle()
        {
            if (_isIdle) return;

            _isIdle = true;
            _playerView.animator.SetBool("Running", false);
        }
        public void Move(float xValue)
        {
            _isIdle = false;
            var velocity = new Vector2(xValue * _playerConfig.moveSpeed, _playerView.rigidBody.linearVelocity.y);
            _playerView.rigidBody.linearVelocity = velocity;
            _playerView.animator.SetBool("Running", true);
        }

        public void Jump()
        {
            if (!_isGrounded) return;
            
            //_isGrounded = false;
            _isJumping = true;
            _playerView.animator.SetBool("Idle", true);

            _playerView.rigidBody.AddForce(new Vector2(0, _playerConfig.jumpForce), ForceMode2D.Force);
        }

        public void SetActive(bool active)
        {
            _playerView.gameObject.SetActive(active);
            if (active)
            {
                _playerView.OnUpdate = Update;
                _playerView.rigidBody.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                _playerView.OnUpdate = null;
                _playerView.rigidBody.bodyType = RigidbodyType2D.Kinematic;
                _playerView.rigidBody.linearVelocity = Vector2.zero;
                _playerView.transform.localPosition = _startPosition;
            }
        }
        
        private void CheckGrounded()
        {
            _isGrounded = Physics2D.OverlapCircle(_playerView.groundCheck.position, 
                                                        _playerConfig.groundCheckRadius,
                                                     _playerConfig.groundLayer);
        
            if (_isGrounded)
            {
                //_lastGroundedTime = _playerConfig.coyoteTime;
                _isJumping = false;
            }
        }

        private void Update()
        {
            CheckGrounded();
            if (_isJumping && _playerView.rigidBody.linearVelocityY <= 0)
            {
                _playerView.rigidBody.gravityScale = _playerConfig.afterJumpGravity;
            }
            else if(!_isJumping)
            {
                _playerView.rigidBody.gravityScale = _playerConfig.defaultGravity;
            }
        }
    }
}
