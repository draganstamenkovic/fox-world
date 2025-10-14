using Gui.Screens.Views;

namespace Gui.Screens.Controllers
{
    public interface IScreenController
    {
        string ID { get; }
        void SetView(IScreenView view);
        void Initialize(IScreenManager screenManager);
    }
}