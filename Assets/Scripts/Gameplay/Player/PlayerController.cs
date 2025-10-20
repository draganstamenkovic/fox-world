using Audio.Managers;
using Cameras;
using Configs;
using DefaultNamespace;
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
        private ParticleSystem _landParticle;
        
        private Vector3 _startPosition;
        
        private bool _isGrounded;
        private bool _isJumping;
        private bool _isIdle;

        private bool _jumpParticlePlayed;
        private float _lastParticleTime;
        
        public PlayerController(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        public void Initialize(Transform gameplayParent)
        {
            var player = _objectResolver.Instantiate(_playerConfig.playerPrefab, gameplayParent);
            player.SetActive(false);
            _playerView = player.GetComponent<PlayerView>();

            _playerView.rigidBody.gravityScale = _playerConfig.defaultGravity;
            _landParticle = _objectResolver.Instantiate(_playerConfig.onLandParticle).GetComponent<ParticleSystem>();
        }
        public void Idle()
        {
            if (_isIdle || _isJumping) return;

            _isIdle = true;
            _playerView.animator.SetBool(AnimationIds.Run, false);
            _playerView.animator.SetBool(AnimationIds.Jump, false);
            _playerView.animator.SetBool(AnimationIds.Idle, true);
        }
        public void Move(float xValue)
        {
            _isIdle = false;
            _playerView.spriteRenderer.flipX = !(xValue > 0);
            
            var velocity = new Vector2(xValue * _playerConfig.moveSpeed, _playerView.rigidBody.linearVelocity.y);
            _playerView.rigidBody.linearVelocity = velocity;
            
            if (_isJumping) return;
            
            _playerView.animator.SetBool(AnimationIds.Run, true);
            _playerView.animator.SetBool(AnimationIds.Jump, false);
            _playerView.animator.SetBool(AnimationIds.Idle, false);
        }

        public void Jump()
        {
            if (!_isGrounded) return;
            
            _playerView.animator.SetBool(AnimationIds.Jump, true);
            _playerView.animator.SetBool(AnimationIds.Run, false);
            _playerView.animator.SetBool(AnimationIds.Idle, false);
            
            _isGrounded = false;
            _isJumping = true;

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
            bool wasGrounded = _isGrounded;
            _isGrounded = Physics2D.OverlapCircle(_playerView.groundCheck.position, 
                _playerConfig.groundCheckRadius,
                _playerConfig.groundLayer);
        
            // Just landed
            if (_isGrounded && !wasGrounded)
            {
                _isJumping = false;
                
                if (Time.time > _lastParticleTime + 0.2f)
                {
                    _playerView.animator.SetBool(AnimationIds.Jump, false);
                    _playerView.animator.SetBool(AnimationIds.Idle, true);
                    _landParticle.transform.position = _playerView.dustParticleTransform.position;
                    _landParticle.Play();
                    _lastParticleTime = Time.time;
                }
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
