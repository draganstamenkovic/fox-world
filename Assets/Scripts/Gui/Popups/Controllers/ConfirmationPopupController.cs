using Gui.Popups.Views;
using UnityEngine;

namespace Gui.Popups.Controllers
{
    public class ConfirmationPopupController : IPopupController
    {
        private IPopupManager _popupManager;
        private ConfirmationPopupView _view;
        public string ID => PopupIds.ConfirmationPopup;
        public void SetView(IPopupView view)
        {
            _view = view as ConfirmationPopupView;
        }

        public void Initialize(IPopupManager popupManager)
        {
            Debug.Log("Initializing Confirmation Popup");
            _popupManager = popupManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RemoveListeners()
        {
        }

        private void RegisterListeners()
        {
        }

        private void HidePopup()
        {
            _popupManager.HidePopup(PopupIds.ConfirmationPopup);
        }
    }
}