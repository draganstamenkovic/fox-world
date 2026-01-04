using Gui.Popups.Views;
using Gui.Screens;
using Message;
using Message.Messages;
using UnityEngine;
using VContainer;

namespace Gui.Popups.Controllers
{
    public class GameOverPopupController : IPopupController
    {
        [Inject] private readonly IMessageBroker _messageBroker;
        private IPopupManager _popupManager;
        private GameOverPopupView _view;
        public string ID => PopupIds.GameOverPopup;

        public void SetView(IPopupView view)
        {
            _view = view as GameOverPopupView;
        }

        public void Initialize(IPopupManager popupManager)
        {
            _popupManager = popupManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }
        private void RegisterListeners()
        {
            _view.restartButton.onClick.AddListener(OnRestartButtonClicked);
            _view.mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            _view.quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void RemoveListeners()
        {
            _view.restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _view.mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
            _view.quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }
        
        private void OnQuitButtonClicked()
        {
            _messageBroker.Publish(new QuitGameMessage());
        }

        private void OnMainMenuButtonClicked()
        {
            _messageBroker.Publish(new GameOverMessage());
            _popupManager.HidePopup(ID);
            _messageBroker.Publish(new ShowScreenMessage(GuiScreenIds.MainMenuScreen));
        }

        private void OnRestartButtonClicked()
        {
            Debug.Log("Restart");
        }
    }
}
