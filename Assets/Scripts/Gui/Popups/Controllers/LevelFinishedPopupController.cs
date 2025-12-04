using Gui.Popups.Views;
using Gui.Screens;
using Message;
using Message.Messages;
using UnityEngine;
using VContainer;

namespace Gui.Popups.Controllers
{
    public class LevelFinishedPopupController : IPopupController
    {
        [Inject] private readonly IMessageBroker _messageBroker;
        private IPopupManager _popupManager;
        private LevelFinishedPopupView _view;
        public string ID => PopupIds.LevelFinishedPopup;

        public void SetView(IPopupView view)
        {
            _view = view as LevelFinishedPopupView;
        }

        public void Initialize(IPopupManager popupManager)
        {
           Debug.Log("Initializing LevelFinished Popup");
            _popupManager = popupManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
            _view.quitButton.onClick.AddListener(OnQuitButtonClicked);
            _view.continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            _popupManager.HidePopup(PopupIds.LevelFinishedPopup);
            _messageBroker.Publish(new NextLevelMessage());
        }

        private void OnQuitButtonClicked()
        {
            _popupManager.HidePopup(PopupIds.LevelFinishedPopup);
            _messageBroker.Publish(new ShowScreenMessage(GuiScreenIds.MainMenuScreen));
        }

        private void RemoveListeners()
        {
            _view.quitButton.onClick.RemoveListener(OnQuitButtonClicked);
            _view.continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }
    }
}
