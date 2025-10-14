using Gui.Screens.Views;
using UnityEngine;

namespace Gui.Screens.Controllers
{
    public class SettingsScreenController : IScreenController
    {
        private SettingsScreenView _view;
        private IScreenManager _screenManager;
        public string ID => GuiScreenIds.SettingsScreen;

        public void SetView(IScreenView view)
        {
            _view = view as SettingsScreenView;
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
