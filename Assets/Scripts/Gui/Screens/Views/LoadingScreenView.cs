using UnityEngine;

namespace Gui.Screens.Views
{
    public class LoadingScreenView : ScreenView
    {
        public override void Initialize()
        {
            ID = GuiScreenIds.LoadingScreen;
            Debug.Log("LoadingScreenView.Initialize");
        }
    }
}