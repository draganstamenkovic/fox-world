using Audio.Managers;
using Gameplay;
using Gui;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Bootstrap : IStartable
{
    [Inject] private GuiManager _guiManager;
    [Inject] private IGameManager _gameManager;
    [Inject] private IAudioManager _audioManager;

    public void Start()
    {
        Prepare();
        _audioManager.Initialize();
        _gameManager.Initialize();
        _guiManager.Initialize();
    }
    private void Prepare()
    {
        Application.targetFrameRate = 60;
    }
}
