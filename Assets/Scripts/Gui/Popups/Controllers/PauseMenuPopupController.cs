using Gui.Popups.Views;
using Message;
using Message.Messages;
using UnityEngine;
using VContainer;

namespace Gui.Popups.Controllers
{
    public class PauseMenuPopupController : IPopupController
    {
        [Inject] private readonly IMessageBroker _messageBroker;
        private IPopupManager _popupManager;
        private PauseMenuPopupView _view;
        public string ID => PopupIds.PauseMenuPopup;

        public void SetView(IPopupView view)
        {
            _view = view as PauseMenuPopupView;
        }

        public void Initialize(IPopupManager popupManager)
        {
           Debug.Log("Initializing PauseMenu Popup");
            _popupManager = popupManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
            _view.continueButton.onClick.AddListener(OnContinueButtonClicked);
            _view.quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void RemoveListeners()
        {
            _view.continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _view.quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }
        
        private void OnContinueButtonClicked()
        {
            _messageBroker.Publish(new PauseGameMessage(false));
            _popupManager.HidePopup(ID);
        }
    }
}
