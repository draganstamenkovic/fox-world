using Data.Save;
using Gui.Screens.Views;
using UnityEngine;
using VContainer;

namespace Gui.Screens.Controllers
{
    public class MainMenuScreenController : IScreenController
    {
        [Inject] private ISaveManager _saveManager;
        private IScreenManager _screenManager;
        
        private MainMenuScreenView _view;
        private int _promotionPopupCounter;
        public string ID => GuiScreenIds.MainMenuScreen;
        public void SetView(IScreenView view)
        {
            _view = view as MainMenuScreenView;
        }

        public void Initialize(IScreenManager screenManager)
        {
            _screenManager = screenManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
            _view.playButton.onClick.AddListener(OnPlayButtonClick);
            _view.settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _view.quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnQuitButtonClick()
        {
            Application.Quit();
        }

        private void OnSettingsButtonClick()
        {
            _screenManager.ShowScreen(GuiScreenIds.SettingsScreen);
        }

        private void OnPlayButtonClick()
        {
            _screenManager.ShowScreen(GuiScreenIds.LevelSelectScreen);
        }

        private void RemoveListeners()
        {
            _view.playButton.onClick.RemoveListener(OnPlayButtonClick);
            _view.settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _view.quitButton.onClick.RemoveListener(OnQuitButtonClick);
        }
    }
}