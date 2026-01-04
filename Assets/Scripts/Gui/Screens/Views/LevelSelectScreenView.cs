using UnityEngine.UI;

namespace Gui.Screens.Views
{
    public class LevelSelectScreenView : ScreenView
    {
        public Button backButton;
        public LevelButton[] levelButtons;
        public override void Initialize()
        {
            ID = GuiScreenIds.LevelSelectScreen;
        }
    }
}
