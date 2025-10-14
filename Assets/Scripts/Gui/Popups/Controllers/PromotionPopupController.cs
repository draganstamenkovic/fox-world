using Gui.Popups.Views;
using UnityEngine;

namespace Gui.Popups.Controllers
{
    public class PromotionPopupController : IPopupController
    {
        private IPopupManager _popupManager;
        private PromotionPopupView _view;
        public string ID => PopupIds.PromotionPopup;

        public void SetView(IPopupView view)
        {
            _view = view as PromotionPopupView;
        }

        public void Initialize(IPopupManager popupManager)
        {
            _popupManager = popupManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
            _view.BackgroundButton.onClick.AddListener(HidePopup);
            _view.BuyButton.onClick.AddListener(OnBuyButtonClicked);
            _view.CloseButton.onClick.AddListener(HidePopup);
        }

        private void RemoveListeners()
        {
            _view.BackgroundButton.onClick.RemoveListener(HidePopup);
            _view.BuyButton.onClick.RemoveListener(OnBuyButtonClicked);
            _view.CloseButton.onClick.RemoveListener(HidePopup);
        }

        private void HidePopup()
        {
            _popupManager.HidePopup(PopupIds.PromotionPopup);
        }
        private void OnBuyButtonClicked()
        {
            Debug.Log("OnBuyButtonClicked");
        }
    }
}
