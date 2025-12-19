using UnityEngine;
using UnityEngine.UI;

namespace Gui.Popups.Views
{
    public class GameOverPopupView : PopupView
    {
        public Button restartButton;
        public Button mainMenuButton;
        public Button quitButton;
        public override void Initialize()
        {
            base.Initialize();
            ID = PopupIds.GameOverPopup;
        }
    }
}
