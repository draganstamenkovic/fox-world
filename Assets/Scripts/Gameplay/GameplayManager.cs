using System;
using System.Collections.Generic;
using Cameras;
using Data.Load;
using Gameplay.Background;
using Gameplay.Level;
using Gameplay.Player;
using Gui.Screens;
using Input;
using Message;
using Message.Messages;
using R3;
using UnityEngine;
using VContainer;

namespace Gameplay
{
    public class GameplayManager : IGameplayManager
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
        
        private bool _isPaused;
        
        public async void Initialize()
        {
            _messageBroker.Publish(new LoadingMessage("Loading player...", true));
            await Awaitable.WaitForSecondsAsync(0.1f);
            _playerController.Initialize();
            
            _messageBroker.Publish(new LoadingMessage("Loading camera...", true));
            await Awaitable.WaitForSecondsAsync(0.1f);
            _cameraManager.Initialize();
            
            _messageBroker.Publish(new LoadingMessage("Loading data...", true));
            await Awaitable.WaitForSecondsAsync(0.1f);
            _loadManager.Initialize();
            
            _messageBroker.Publish(new LoadingMessage("Loading controls...", true));
            await Awaitable.WaitForSecondsAsync(0.1f);
            _inputManager.Initialize(_playerController);
            
            _messageBroker.Publish(new LoadingMessage("Loading levels...", true));
            await Awaitable.WaitForSecondsAsync(0.1f);
            _levelManager.Initialize();
            
            _messageBroker.Publish(new LoadingMessage("Loading backgrounds...", true));
            await Awaitable.WaitForSecondsAsync(0.1f);
            _backgroundManager.Initialize();
            SubscribeToEvents();
            
            _messageBroker.Publish(new LoadingMessage(string.Empty, false));
            _messageBroker.Publish(new ShowScreenMessage(GuiScreenIds.MainMenuScreen));
        }

        private void SubscribeToEvents()
        {
            _disposableMessages.Add(_messageBroker.Receive<PlayGameMessage>().Subscribe(message =>
            {
                Play();
            }));
            _disposableMessages.Add(_messageBroker.Receive<PauseGameMessage>().Subscribe(message =>
            {
                if (message.Paused)
                    Pause();
                else
                    Resume();
            }));
            _disposableMessages.Add(_messageBroker.Receive<GameOverMessage>().Subscribe(message =>
            {
                _inputManager.SetActive(false);
            }));
        }

        public async void Play()
        {
            await _levelManager.LoadLevel();
            _playerController.SetActive(true);
            _inputManager.SetActive(true);
            _cameraManager.FollowPlayer(_playerController.GetTransform());
            _cameraManager.SetCinemachineConfiner(_levelManager.GetLevelCollider());
        }

        public void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0f;
            _inputManager.SetActive(false);
        }

        public void Resume()
        {
            if (!_isPaused) return;
            
            _isPaused = false;
            Time.timeScale = 1f;
            _inputManager.SetActive(true);
        }

        public void Stop()
        {
            if (_isPaused)
            {
                Time.timeScale = 1f;
                _isPaused = false;
            }
            _levelManager.DestroyActiveLevel();
            _playerController.SetActive(false);
        }

        public void Restart()
        {
            Debug.Log("Restart");
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