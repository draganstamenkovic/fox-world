using Gui.Screens.Views;
using UnityEngine;

namespace Gui.Screens.Controllers
{
    public class LoadingScreenController : IScreenController
    {
        private IScreenManager _screenManager;
        private LoadingScreenView _view;
        public string ID => GuiScreenIds.LoadingScreen;
        public void SetView(IScreenView view)
        {
            _view = view as LoadingScreenView;
        }

        public void Initialize(IScreenManager screenManager)
        {
            _screenManager = screenManager;
            Debug.Log("Initializing Loading Screen");
        }
    }
}