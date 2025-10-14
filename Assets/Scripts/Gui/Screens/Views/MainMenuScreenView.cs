using UnityEngine;
using UnityEngine.UI;

namespace Gui.Screens.Views
{
    public class MainMenuScreenView : ScreenView
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _saveButton;
        
        public Button SettingsButton => _settingsButton;
        public Button PlayButton => _playButton;
        public Button SaveButton => _saveButton;
        public override void Initialize()
        {
            ID = GuiScreenIds.MainMenuScreen;
        }
    }
}