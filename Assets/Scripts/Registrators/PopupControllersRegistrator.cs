using Gui.Popups.Controllers;
using VContainer;

namespace Registrators
{
    public class PopupControllersRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.Register<ConfirmationPopupController>(Lifetime.Singleton).As<IPopupController>();
            builder.Register<PromotionPopupController>(Lifetime.Singleton).As<IPopupController>();
            builder.Register<LevelFinishedPopupController>(Lifetime.Singleton).As<IPopupController>();
        }
    }
}