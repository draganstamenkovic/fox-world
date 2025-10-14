using UnityEngine;
using UnityEngine.UI;

namespace Gui.Popups.Views
{
    public class LevelFinishedPopupView : PopupView
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _quitButton;
        
        public Button ContinueButton => _continueButton;
        public Button QuitButton => _quitButton;
        public override void Initialize()
        {
            base.Initialize();
            ID = PopupIds.LevelFinishedPopup;
        }
    }
}
