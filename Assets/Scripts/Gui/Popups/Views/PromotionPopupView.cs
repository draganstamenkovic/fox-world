using UnityEngine;
using UnityEngine.UI;

namespace Gui.Popups.Views
{
    public class PromotionPopupView : PopupView
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _closeButton;
        public Button BuyButton => _buyButton;
        public Button CloseButton => _closeButton;
        public override void Initialize()
        {
            base.Initialize();
            ID = PopupIds.PromotionPopup;
        }

    }
}
