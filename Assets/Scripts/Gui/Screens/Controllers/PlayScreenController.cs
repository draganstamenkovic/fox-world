using Gui.Screens.Views;
using UnityEngine;

namespace Gui.Screens.Controllers
{
    public class PlayScreenController : IScreenController
    {
        private PlayScreenView _view;
        private IScreenManager _screenManager;
        public string ID => GuiScreenIds.PlayScreen;

        public void SetView(IScreenView view)
        {
            _view = view as PlayScreenView;
        }

        public void Initialize(IScreenManager screenManager)
        {
           _screenManager = screenManager;
            _view.OnShow = RegisterListeners;
            _view.OnHidden = RemoveListeners;
        }

        private void RegisterListeners()
        {
        }

        private void RemoveListeners()
        {
        }
    }
}
