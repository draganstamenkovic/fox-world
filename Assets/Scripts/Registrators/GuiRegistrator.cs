using Gui;
using Gui.Popups;
using Gui.Popups.Builder;
using Gui.Screens;
using VContainer;
using VContainer.Unity;

namespace Registrators
{
    public class GuiRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GuiManager>();
            builder.Register<IScreenManager, ScreenManager>(Lifetime.Singleton);
            builder.Register<IPopupBuilder, PopupBuilder>(Lifetime.Singleton);
            builder.Register<IPopupManager, PopupManager>(Lifetime.Singleton);
        }
    }
}
