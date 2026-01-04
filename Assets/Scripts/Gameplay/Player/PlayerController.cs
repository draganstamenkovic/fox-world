using Audio;
using Configs;
using Gameplay.Collectables;
using Gameplay.Enemies;
using Gui.Popups;
using Message;
using Message.Messages;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Player
{
    public class PlayerController : IPlayerController
    {
        [Inject] private PlayerConfig _playerConfig;
        [Inject] private IMessageBroker _messageBroker;
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
        public Transform GetTransform() => _playerView.transform;

        public void Initialize()
        {
            var player = _objectResolver.Instantiate(_playerConfig.playerPrefab);
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
            _playerView.transform.localScale = xValue >= 0 ? new  Vector3(1,1,1) : new Vector3(-1, 1, 1);
            
            var rb = _playerView.rigidBody;
            var v = rb.linearVelocity;

            v.x = xValue * _playerConfig.moveSpeed;

            rb.linearVelocity = v;
            
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
                _playerView.animator.Play(AnimationIds.Idle);
                _playerView.OnUpdate = Update;
                _playerView.OnCollisionEnter = OnCollisionEnter2D;
                _playerView.OnTriggerEnter = OnTriggerEnter2D;
                _playerView.rigidBody.bodyType = RigidbodyType2D.Dynamic;
                _playerView.transform.localPosition = _playerConfig.startPosition;
            }
            else
                FreezePlayer();
        }

        private void FreezePlayer()
        {
            _playerView.OnUpdate = null;
            _playerView.OnCollisionEnter = null;
            _playerView.OnTriggerEnter = null;
            _playerView.rigidBody.bodyType = RigidbodyType2D.Kinematic;
            _playerView.rigidBody.linearVelocity = Vector2.zero;
        }

        private void CheckGrounded()
        {
            var wasGrounded = _isGrounded;
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
                    _landParticle.transform.position = _playerView.dustParticleTransform.position;
                    _landParticle.Play();
                    _lastParticleTime = Time.time;
                }
            }
        }

        private void Update()
        {
            CheckGrounded();

            if (_playerView.rigidBody.linearVelocityY <= 0)
                _playerView.rigidBody.gravityScale = _playerConfig.afterJumpGravity;
            else
            {
                _playerView.rigidBody.gravityScale = _playerConfig.defaultGravity;
            }
        }
        private void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            if (triggerCollider.CompareTag(TagIds.Collectable))
            {
                _messageBroker.Publish(new PlaySfxMessage(AudioIds.CollectablePickUp));
                var collectable = triggerCollider.gameObject.GetComponent<CollectableItem>();
                _messageBroker.Publish(new CollectedItemMessage(collectable));
            }

            if (triggerCollider.CompareTag(TagIds.DeathHole))
            {
                _messageBroker.Publish(new PlaySfxMessage(AudioIds.PlayerDied));
                _messageBroker.Publish(new ShowPopupMessage(PopupIds.GameOverPopup));
                _messageBroker.Publish(new GameOverMessage());
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(TagIds.Enemy))
            {
                var contact = collision.GetContact(0);
                var normal = contact.normal;
                var enemy = collision.gameObject.GetComponent<Enemy>();
                if (normal.y > _playerConfig.topHitThreshold)
                {
                    enemy.Die();

                    _playerView.rigidBody.linearVelocity = new Vector2(_playerView.rigidBody.linearVelocity.x,
                        _playerConfig.jumpForceOnKill);
                }
                else
                {
                    enemy.SetColliderState(false);
                    Die();
                }
            }
        }

        private void Die()
        { 
            _playerView.animator.Play(AnimationIds.Dead);
            _messageBroker.Publish(new PlaySfxMessage(AudioIds.PlayerDied));
            _messageBroker.Publish(new DisableInputMessage());
            _playerView.ShrinkPlayer(() =>
            {
                _messageBroker.Publish(new ShowPopupMessage(PopupIds.GameOverPopup));
            });
        }
    }
}
