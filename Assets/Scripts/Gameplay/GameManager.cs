using System;
using System.Collections.Generic;
using Cameras;
using Data.Load;
using Gameplay.Background;
using Gameplay.Level;
using Gameplay.Player;
using Input;
using Message;
using Message.Messages;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay
{
    public class GameManager : IGameManager
    {
        [Inject] private readonly InputManager _inputManager;
        [Inject] private readonly BackgroundManager _backgroundManager;
        [Inject] private readonly ICameraManager _cameraManager;
        [Inject] private readonly ILoadManager _loadManager;
        [Inject] private readonly IPlayerController _playerController;
        [Inject] private readonly ILevelManager _levelManager;
        [Inject] private readonly IScoreManager _scoreManager;

        [Inject] private readonly IMessageBroker _messageBroker;
        private readonly List<IDisposable> _disposableMessages = new();
        
        private Transform _gameplayParent;
        private bool _isPaused;
        
        public void Initialize()
        {
            _playerController.Initialize(_gameplayParent);
            _cameraManager.Initialize(_playerController.GetTransform());
            _loadManager.Initialize();
            _inputManager.Initialize(_playerController);
            _levelManager.Initialize();
            _backgroundManager.Initialize();
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _disposableMessages.Add(_messageBroker.Receive<PlayGameMessage>().Subscribe(message =>
            {
                Play();
            }));
            _disposableMessages.Add(_messageBroker.Receive<PauseGameMessage>().Subscribe(message =>
            {
                if(message.Paused)
                    Pause();
                else
                    Resume();
            }));
        }

        public void Play()
        {
            _playerController.SetActive(true);
            _inputManager.SetActive(true);
        }

        public void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            if (!_isPaused) return;
            
            _isPaused = false;
            Time.timeScale = 1f;
        }

        public void Stop()
        {
            if (_isPaused)
            {
                Time.timeScale = 1f;
                _isPaused = false;
            }
            _playerController.SetActive(false);
            _inputManager.SetActive(false);
        }
        
        public void Quit()
        {
            Stop();
            Cleanup();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID || UNITY_IOS
            Application.Quit();
#endif
        }

        private void Cleanup()
        {
            foreach (var disposable in _disposableMessages) disposable.Dispose();
        }
    }
}