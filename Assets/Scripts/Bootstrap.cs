using Audio.Managers;
using Gameplay;
using Gui;
using Helpers.RuntimeInfo;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class Bootstrap : IStartable
{
    [Inject] private GuiManager _guiManager;
    [Inject] private IGameManager _gameManager;
    [Inject] private IAudioManager _audioManager;
    [Inject] private IRuntimeInformation _runtimeInformation;

    public void Start()
    {
        Prepare();
        _runtimeInformation.Initialize();
        _audioManager.Initialize();
        _gameManager.Initialize();
        _guiManager.Initialize();
    }
    private void Prepare()
    {
        Application.targetFrameRate = 60;
    }
}
