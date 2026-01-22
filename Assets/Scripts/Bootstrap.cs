using Audio.Managers;
using Gameplay;
using Gui;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Bootstrap : IStartable
{
    [Inject] private GuiManager _guiManager;
    [Inject] private IGameplayManager _gameplayManager;
    [Inject] private IAudioManager _audioManager;
    public async void Start()
    {
        Prepare();
        _guiManager.Initialize();
        await Awaitable.WaitForSecondsAsync(0.3f);
        _audioManager.Initialize();
        await Awaitable.WaitForSecondsAsync(0.3f);
        _gameplayManager.Initialize();
    }
    private void Prepare()
    {
        Application.targetFrameRate = 60;
        
    }
}
