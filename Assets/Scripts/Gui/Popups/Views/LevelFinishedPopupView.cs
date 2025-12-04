using UnityEngine;
using UnityEngine.UI;

namespace Gui.Popups.Views
{
    public class LevelFinishedPopupView : PopupView
    {
        public Button continueButton;
        public Button quitButton;
        public override void Initialize()
        {
            base.Initialize();
            ID = PopupIds.LevelFinishedPopup;
        }
    }
}
