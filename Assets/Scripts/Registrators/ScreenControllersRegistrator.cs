using Gui.Screens.Controllers;
using VContainer;

namespace Registrators
{
    public class ScreenControllersRegistrator
    {
        public static void Register(IContainerBuilder builder)
        {
            builder.Register<LoadingScreenController>(Lifetime.Singleton).As<IScreenController>();
            builder.Register<MainMenuScreenController>(Lifetime.Singleton).As<IScreenController>();
            builder.Register<PlayScreenController>(Lifetime.Singleton).As<IScreenController>();
            builder.Register<SettingsScreenController>(Lifetime.Singleton).As<IScreenController>();
            builder.Register<LevelSelectScreenController>(Lifetime.Singleton).As<IScreenController>();
        }
    }
}