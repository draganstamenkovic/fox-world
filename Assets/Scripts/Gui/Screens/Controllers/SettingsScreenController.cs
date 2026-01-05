using Audio.Managers;
using Gui.Screens.Views;
using Message;
using Message.Messages;
using UnityEngine;
using VContainer;

namespace Gui.Screens.Controllers
{
    public class SettingsScreenController : IScreenController
    {
        [Inject] private readonly IAudioManager _audioManager;
        private SettingsScreenView _view;
        private IScreenManager _screenManager;
        public string ID => GuiScreenIds.SettingsScreen;

        public void SetView(IScreenView view)
        {
            _view = view as SettingsScreenView;
        }

        public void Initialize(IScreenManager screenManager)
        { 
            _screenManager = screenManager;
            _view.OnShow = () =>
            {
                _view.musicToggle.isOn = !_audioManager.BackgroundMusicSource().mute;
                _view.soundToggle.isOn = !_audioManager.SfxSource().mute;
                RegisterListeners();
            };
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
            _view.backButton.onClick.AddListener(OnBackButtonClick);
            _view.soundToggle.onValueChanged.AddListener(OnSoundToggleValueChanged);
            _view.musicToggle.onValueChanged.AddListener(OnMusicToggleValueChanged);
        }
        
        private void RemoveListeners()
        {
            _view.backButton.onClick.RemoveListener(OnBackButtonClick);
            _view.soundToggle.onValueChanged.RemoveListener(OnSoundToggleValueChanged);
            _view.musicToggle.onValueChanged.RemoveListener(OnMusicToggleValueChanged);
        }

        private void OnMusicToggleValueChanged(bool toggleValue)
        {
            _audioManager.BackgroundMusicSource().mute = !toggleValue;
        }

        private void OnSoundToggleValueChanged(bool toggleValue)
        {
            _audioManager.SfxSource().mute = !toggleValue;
        }
        
        private void OnBackButtonClick()
        {
            _screenManager.ShowScreen(GuiScreenIds.MainMenuScreen);
        }
    }
}
