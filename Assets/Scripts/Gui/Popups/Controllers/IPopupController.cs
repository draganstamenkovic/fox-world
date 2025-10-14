using Gui.Popups.Views;

namespace Gui.Popups.Controllers
{
    public interface IPopupController
    {
        string ID { get; }
        void SetView(IPopupView view);
        void Initialize(IPopupManager popupManager);
    }
}