using System;
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
        public PlayerController(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        public void Initialize(Transform gameplayParent)
        {
            var activePlayerSkinId = "";
            var player = _objectResolver.Instantiate(_playerConfig
                .GetActivePlayerSkin(activePlayerSkinId), gameplayParent);
            player.SetActive(false);
            var positionY = -_cameraManager.GetOrthographicSize()
                            + player.transform.localScale.y 
                            + _playerConfig.offsetPositionY;
            _startPosition = new Vector3(0,positionY,0);
            player.transform.position = _startPosition;

            _playerView = player.GetComponent<PlayerView>();
        }

        public void Move(MovementDirection direction)
        {
            var velocity = direction switch
            {
                MovementDirection.Left => new Vector2(-_playerConfig.speed, 0),
                MovementDirection.Right => new Vector2(_playerConfig.speed, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            _playerView.Rigidbody.linearVelocity = velocity;
        }

        public void Move(Vector2 direction)
        {
            // TODO: Implement missing logic here
        }

        public void SetActive(bool active)
        {
            _playerView.gameObject.SetActive(active);
            if (active)
            {
                _playerView.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                _playerView.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
                _playerView.Rigidbody.linearVelocity = Vector2.zero;
                _playerView.transform.localPosition = _startPosition;
            }
        }
    }
}
