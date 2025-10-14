using Data.Save;
using Gui.Popups;
using Gui.Screens.Views;
using VContainer;

namespace Gui.Screens.Controllers
{
    public class MainMenuScreenController : IScreenController
    {
        [Inject] private IPopupManager _popupManager;
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
            _view.OnShown = ShowOffer;
            _view.OnHidden = RemoveListeners;
        }

        private void ShowOffer()
        {
            if (_promotionPopupCounter == 0)
            {
                _popupManager.ShowPopupScreen(PopupIds.PromotionPopup);
                _promotionPopupCounter++;
            }
        }

        private void RegisterListeners()
        {
            _view.PlayButton.onClick.AddListener(OnPlayButtonClick);
            _view.SettingsButton.onClick.AddListener(OnSettingsButtonClick);
            _view.SaveButton.onClick.AddListener(OnSaveButtonClick);
        }

        private void OnSaveButtonClick()
        {
            _saveManager.Save();
        }

        private void OnSettingsButtonClick()
        {
            _screenManager.ShowScreen(GuiScreenIds.SettingsScreen);
        }

        private void OnPlayButtonClick()
        {
            _screenManager.ShowScreen(GuiScreenIds.PlayScreen);
        }

        private void RemoveListeners()
        {
            _view.PlayButton.onClick.RemoveListener(OnPlayButtonClick);
            _view.SettingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _view.SaveButton.onClick.RemoveListener(OnSaveButtonClick);
        }
    }
}